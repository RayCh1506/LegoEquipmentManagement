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
import { useSignalRWithId } from "@/hooks/signalR";

export default function EquipmentPage() {    
    const params = useParams<{ id: string }>();
    const queryClient = getQueryClient();
    const [selectedOrder, setSelectedOrder] = useState<number | undefined>(undefined);
    const { user } = useUser();
    const isSupervisor = user === "Supervisor";

    useSignalRWithId(params.id);

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
        onError: (error) => toast.error(error.message, { duration: 5000, position: "top-center" }),
    });

    const startMutation = useMutation({
        mutationFn: ({ orderId }: { orderId: number | undefined }) => startEquipment(params.id, orderId),
        onError: (error) => toast.error(error.message, { duration: 5000, position: "top-center" }),
    });

    const stopMutation = useMutation({
        mutationFn: () => stopEquipment(params.id),
        onSuccess: () => {
            queryClient.invalidateQueries({ queryKey: [`equipment-${params.id}`] });
            queryClient.invalidateQueries({ queryKey: ["equipment-all"] });
        },
        onError: (error) => toast.error(error.message, { duration: 5000, position: "top-center" }),
    });

    if (!user) {
        return <div className="text-center text-2xl">You are not logged in, please log in to access this page.</div>;
    }

    if (isLoading) return <p className="text-center text-2xl">Loading...</p>;
    if (error) return <p className="text-center text-2xl">Error loading data from the equipment management system</p>;

    if (data) {
        return (
            <div className="flex justify-center min-h-[60vh] bg-gray-100 p-6">
                <div className="w-full max-w-4xl border bg-white rounded-lg shadow-lg p-6">

                    <EquipmentData equipment={data} />

                    {data.operationalInformation.isOperational && !data.operationalInformation.faultMessage && (
                        <div className="mt-6 flex justify-center gap-6">
                            <button
                                className={`px-6 py-3 rounded-md font-bold transition ${
                                    data.generalInformation.state !== "RED"
                                        ? "*:bg-gray-400 text-gray-700 cursor-not-allowed"
                                        : "bg-green-500 hover:bg-green-600 text-white"
                                }`}
                                onClick={() => data.generalInformation.state === "RED" && startMutation.mutate({ orderId: selectedOrder })}
                                disabled={data.generalInformation.state !== "RED" || startMutation.isPending}
                            >
                                {startMutation.isPending ? "Starting..." : "Start Equipment"}
                            </button>

                            <button
                                className={`px-6 py-3 rounded-md font-bold transition ${
                                    data.generalInformation.state !== "GREEN"
                                        ? "bg-gray-400 text-gray-700 cursor-not-allowed"
                                        : "bg-red-500 hover:bg-red-600 text-white"
                                }`}
                                onClick={() => data.generalInformation.state === "GREEN" && stopMutation.mutate()}
                                disabled={data.generalInformation.state !== "GREEN" || stopMutation.isPending}
                            >
                                {stopMutation.isPending ? "Stopping..." : "Stop Equipment"}
                            </button>
                        </div>
                    )}

                    <div className="mt-6">
                        <OrderData equipment={data} selectedOrder={selectedOrder} setSelectedOrder={setSelectedOrder} isSupervisor={isSupervisor} />
                    </div>

                    {data.operationalInformation.isOperational && !data.operationalInformation.faultMessage && (
                        <div className="mt-6 text-center">
                            <h2 className="text-lg font-semibold mb-2">Change State:</h2>
                            {changeStateMutation.isPending ? (
                                <p className="text-center text-lg font-semibold">Updating...</p>
                            ) : (
                                <div className="flex justify-center gap-4">
                                    {(["RED", "YELLOW", "GREEN"] as EquipmentState[]).map((newState) => {
                                        const isDisabled = !allowedTransitions[data.generalInformation.state]?.includes(newState);
                                        return (
                                            <button
                                                key={newState}
                                                className={`px-6 py-3 rounded-md font-semibold transition ${
                                                    isDisabled
                                                        ? "bg-gray-400 text-gray-700 cursor-not-allowed"
                                                        : newState === "RED"
                                                        ? "bg-red-500 hover:bg-red-600 text-white"
                                                        : newState === "YELLOW"
                                                        ? "bg-yellow-500 hover:bg-yellow-600 text-black"
                                                        : "bg-green-500 hover:bg-green-600 text-white"
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
                    )}

                    {isSupervisor && (
                        <div className="mt-6">
                            <EquipmentHistory equipment={data} />
                        </div>
                    )}

                </div>
            </div>
        );
    }
}