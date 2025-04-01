import { Equipment } from "@/types/types";
import { AlertTriangle } from "lucide-react";

export default function EquipmentData({ equipment }: { equipment: Equipment }) {
    return (
        <div className="border p-4 rounded-lg shadow-md bg-white w-full">
            <h1 className="text-lg font-semibold text-black">{equipment.generalInformation.name} - OOE: {equipment.operationalInformation.overallEquipmentEffectiveness}%</h1>
            
            <p className="text-sm flex items-center mt-2">
                <span className={`px-2 py-1 text-xs font-bold rounded-full ${
                    equipment.generalInformation.state === 'RED' ? 'bg-red-100 text-red-600' 
                    : equipment.generalInformation.state === 'YELLOW' ? 'bg-yellow-100 text-yellow-600' 
                    : 'bg-green-100 text-green-600'
                }`}>
                    {equipment.generalInformation.state}
                </span>
            </p>
            <div className="mt-3 grid grid-cols-2 gap-4">
                <p className="text-sm text-gray-600">
                    <span className="font-bold">Current Order:</span> {equipment.orderInformation.currentOrder ?? "No current order"}
                </p>
                <p className="text-sm text-gray-600">
                    <span className="font-bold">Location:</span> {equipment.generalInformation.location}
                </p>
                <p className="text-sm flex items-center">
                    {!equipment.operationalInformation.isOperational && <AlertTriangle className="text-red-600 w-5 h-5 mr-1" />} 
                    <span className="font-bold">Operational:</span> {equipment.operationalInformation.isOperational ? 'Yes' : 'No'}
                </p>
                
            </div>
            <p className="text-sm text-gray-600">
                    <span className="font-bold">Operator:</span> {equipment.operationalInformation.operator}
                </p>
        </div>
    );
}