import { Equipment } from "@/types/types";
import { Dispatch, SetStateAction } from "react";

export default function OrderData({
    equipment,
    selectedOrder,
    setSelectedOrder,
    isSupervisor
}: {
    equipment: Equipment;
    selectedOrder: number | undefined;
    setSelectedOrder: Dispatch<SetStateAction<number | undefined>>;
    isSupervisor: boolean;
}) {
    return (
        <div className="mt-4 w-full p-3 rounded-lg border border-gray-300">
            <h3 className="text-md font-semibold text-gray-800">Assigned Orders</h3>

            {equipment.orderInformation.assignedOrders.length > 0 ? (
                <div className="mt-2 flex flex-wrap items-center gap-2">
                    <ul className="flex flex-wrap gap-2">
                        {equipment.orderInformation.assignedOrders.map((orderId) => (
                            <li key={orderId} className="text-sm px-2 py-1 bg-gray-100 rounded-md border">
                                {orderId}
                            </li>
                        ))}
                    </ul>

                    {equipment.generalInformation.state === "RED" && isSupervisor && (
                        <div className="ml-auto">
                            <label className="text-sm font-semibold mr-2">Select Order:</label>
                            <select
                                className="px-3 py-1 border rounded-md bg-white shadow-sm text-sm"
                                value={selectedOrder ?? ""}
                                onChange={(e) => setSelectedOrder(e.target.value ? Number(e.target.value) : undefined)}
                            >
                                <option value="">None</option>
                                {equipment.orderInformation.assignedOrders.map((orderId) => (
                                    <option key={orderId} value={orderId}>{orderId}</option>
                                ))}
                            </select>
                        </div>
                    )}
                </div>
            ) : (
                <p className="text-sm text-gray-600 mt-1">No assigned orders</p>
            )}
        </div>
    );
}