import { Equipment } from "@/types/types";
import { useState } from "react";

export default function EquipmentHistory(props: {equipment: Equipment}){
        const [isExpanded, setIsExpanded] = useState(false);
    return (
        <div className="mt-4 flex flex-col items-center w-full">
            <button 
                className="bg-red-500 text-white px-4 py-2 rounded-md"
                onClick={() => setIsExpanded(!isExpanded)}
            >
                {isExpanded ? "Hide History" : "Show History"}
            </button>

            {isExpanded && (
                <div className="overflow-x-auto w-full max-h-[30vh] overflow-y-scroll flex justify-center mt-2">
                <table className="w-full max-w-4xl border-collapse border border-gray-300">
                    <thead>
                    <tr className="bg-gray-200">
                        <th className="border border-gray-300 px-4 py-2 text-left">From State</th>
                        <th className="border border-gray-300 px-4 py-2 text-left">To State</th>
                        <th className="border border-gray-300 px-4 py-2 text-left">Timestamp</th>
                        <th className="border border-gray-300 px-4 py-2 text-left">Order</th>
                    </tr>
                    </thead>
                    <tbody>
                    {props.equipment.historicStates.map((state, index) => (
                        <tr key={index} className="bg-white even:bg-gray-100">
                        <td className="border border-gray-300 px-4 py-2">{state.fromState}</td>
                        <td className="border border-gray-300 px-4 py-2">{state.toState}</td>
                        <td className="border border-gray-300 px-4 py-2">{state.timeOfChange}</td>
                        <td className="border border-gray-300 px-4 py-2">{state.orderId ?? "N/A"}</td>
                        </tr>
                    ))}
                    </tbody>
                </table>
                </div>
            )}
        </div>
    )
}