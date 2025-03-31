import { Equipment } from "@/types/types";
import { Dispatch, SetStateAction } from "react";

export default function OrderData(props: {equipment: Equipment, selectedOrder: number | undefined, setSelectedOrder : Dispatch<SetStateAction<number | undefined>>}){
    return (
        <div className="mt-4">
            <h3 className="text-sm font-semibold">Assigned Orders:</h3>
            {props.equipment.assignedOrders && props.equipment.assignedOrders.length > 0 ? (
                <div className="mt-4 flex flex-col items-center">
                        <ul className="list-disc pl-4">
                        {props.equipment.assignedOrders.map((orderId) => (
                            <li key={orderId} className="text-sm">{orderId}</li>
                        ))}
                    </ul>
                    {
                        props.equipment.state === "RED" && 
                        (
                            <>
                                <label className="text-sm font-semibold mb-1">Select Order:</label>
                                <select
                                    className="px-3 py-2 border rounded-md text-black w-full max-w-[200px] mb-2"
                                    value={props.selectedOrder ?? ""}
                                    onChange={(e) => props.setSelectedOrder(e.target.value ? Number(e.target.value) : undefined)}
                                >
                                    <option value="">None</option>
                                    {props.equipment.assignedOrders.map((orderId) => (
                                        <option key={orderId} value={orderId}>{orderId}</option>
                                    ))}
                                </select>
                            </>
                        )
                    }
                </div>
            ) :  (
                <p className="text-sm text-gray-500">No assigned orders</p>
            )}
        </div>
    )
}