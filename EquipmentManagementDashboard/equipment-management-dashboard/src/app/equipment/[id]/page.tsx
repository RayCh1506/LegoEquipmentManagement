"use client"

import { fetchEquipment, startEquipment, stopEquipment, updateEquipmentState } from "@/api/api";
import { getQueryClient } from "@/utils/getQueryClient";
import { Equipment } from "@/types/types";
import { useMutation, useQuery } from "@tanstack/react-query";
import { useParams } from "next/navigation";
import { useState } from "react";
import EquipmentData from "@/components/EquipmentData";
import { useUser } from "@/providers/UserContextProvider";
import {EquipmentState} from "@/types/types"
import OrderData from "@/components/OrderData";
import EquipmentHistory from "@/components/EquipmentHistory";
import toast from "react-hot-toast";

export default function EquipmentPage() 
{    
    const params = useParams<{id: string}>();
    const queryClient = getQueryClient();
    const [selectedOrder, setSelectedOrder] = useState<number | undefined>(undefined);
    const {user} = useUser();

    const isSupervisor = user === "Supervisor";

    const allowedTransitions: Record<string, EquipmentState[]> = {
        RED: ["YELLOW"],
        YELLOW: ["RED", "GREEN"],
        GREEN: ["YELLOW"],
    };

    const { data, error, isLoading } = useQuery<Equipment>({
        queryKey: [`equipment-${params.id}`],
        queryFn: () => fetchEquipment(params.id),
    });

    const changeStateMutation = useMutation({
        mutationFn: ({ newState, currentOrder }: { newState: EquipmentState, currentOrder: number | undefined }) => 
            updateEquipmentState({ id: params.id, newState, currentOrder }),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [`equipment-${params.id}`] });
            queryClient.invalidateQueries({ queryKey: ["equipment-all"] });
        },
        onError: (error) => {
            console.error("Failed to update equipment state:", error);
            toast.error(error.message, {
                duration: 5000,
                position: "top-center",
                
            })
        }
    });

    const startMutation = useMutation({
        mutationFn: ({ orderId }: { orderId: number | undefined }) => startEquipment(params.id, orderId),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [`equipment-${params.id}`] });
        },
        onError: (error) => {
            console.error("Failed to start equipment:", error);
            toast.error(error.message, {
                duration: 5000,
                position: "top-center",
                
            })
        }
    });

    const stopMutation = useMutation({
        mutationFn: () => stopEquipment(params.id),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [`equipment-${params.id}`] });
        },
        onError: (error) => {
            console.error("Failed to stop equipment:", error);
            toast.error(error.message, {
                duration: 5000,
                position: "top-center"
                
            })
        }
    });

    if(!user){
        return (
            <div className="text-center text-2xl">
                You are not logged in, please log in to see access the page
            </div>
        )
    }
    
    if (isLoading) return <p className="text-center text-2xl">Loading...</p>;
    if (error) return <p className="text-center text-2xl">Error loading data from the equipment management system</p>;

    if(data != null && data != undefined){
        return  (
            <div className="min-h-fit place-items-center border-4 bg-gray-300 rounded-lg p-4 ml-5 mr-5">
                {data.operationalInformation.isOperational && !data.operationalInformation.faultMessage && (
                    <div className="mt-4 mb-2 flex flex-col items-center w-full">
                        <div className="flex space-x-4">
                            <button
                                className={`text-white px-4 py-2 rounded-md ${data.generalInformation.state !== "RED" ? 'bg-gray-400 text-gray-500 cursor-not-allowed' : 'bg-green-500'}`}
                                onClick={() => data.generalInformation.state === "RED" && startMutation.mutate({ orderId: selectedOrder })}
                                disabled={data.generalInformation.state !== "RED" || startMutation.isPending}
                            >
                                {startMutation.isPending ? "Starting..." : "Start Equipment"}
                            </button>

                            <button
                                className={`text-white px-4 py-2 rounded-md ${data.generalInformation.state !== "GREEN" ? 'bg-gray-400 text-gray-500 cursor-not-allowed' : 'bg-red-500'}`}
                                onClick={() => data.generalInformation.state === "GREEN" && stopMutation.mutate()}
                                disabled={data.generalInformation.state !== "GREEN" || stopMutation.isPending}
                            >
                                {stopMutation.isPending ? "Stopping..." : "Stop Equipment"}
                            </button>
                        </div>
                    </div>
                )}
                <EquipmentData equipment={data}/>
                <OrderData equipment={data} selectedOrder={selectedOrder} setSelectedOrder={setSelectedOrder} isSupervisor={isSupervisor}/>
                <br/>
                {
                    data.operationalInformation.isOperational && !data.operationalInformation.faultMessage && (
                        <div className="mt-4 flex flex-col items-center">
                        <label className="text-sm font-semibold mb-1">Change State:</label>
                        {changeStateMutation.isPending ? (
                            <p className="text-center text-lg font-semibold">Updating...</p>
                        ) : (
                            <div className="flex w-full space-x-2">
                                {(["RED", "YELLOW", "GREEN"] as EquipmentState[]).map((newState) => {
                                    const isDisabled = !allowedTransitions[data.generalInformation.state]?.includes(newState);
                                    return (
                                        <button
                                            key={newState}
                                            className={`flex-1 px-4 py-2 rounded-md font-semibold text-white ${isDisabled ? 'bg-gray-400 text-gray-500 cursor-not-allowed' :
                                                newState === "RED" 
                                                ? "bg-red-500" 
                                                : newState === "YELLOW" 
                                                ? "bg-yellow-500" 
                                                : "bg-green-500"
                                            }`}
                                            onClick={() => !isDisabled && changeStateMutation.mutate({ 
                                                newState, 
                                                currentOrder: selectedOrder ?? data.orderInformation.currentOrder 
                                            })}
                                            disabled={isDisabled}
                                        >
                                            {newState}
                                        </button>
                                    );
                                })}
                            </div>
                        )}
                        </div>
                    )
                }
                {
                    isSupervisor && (
                        <EquipmentHistory equipment={data}/>
                    )
                }
            </div>
            
        )
    }
}