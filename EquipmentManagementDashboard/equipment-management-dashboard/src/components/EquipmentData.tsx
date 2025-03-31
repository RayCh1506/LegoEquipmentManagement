import { Equipment } from "@/types/types";
import { AlertTriangle } from "lucide-react";

export default function EquipmentData(props: {equipment: Equipment}){
    return (
        <>
            <h1 className="text-lg font-semibold text-black items-center">{props.equipment.name}</h1>
            <p className={`text-sm ${props.equipment.state === 'RED' ? 'text-red-500' : props.equipment.state === 'YELLOW' ? 'text-yellow-500' : 'text-green-500'}`}>State: <span className="font-bold">{props.equipment.state}</span></p>
            <p className="text-sm text-gray-600">Current order: <span className="font-bold">{props.equipment.currentOrder ?? "No current order"}</span></p>
            <p className="text-sm text-gray-600">Location: <span className="font-bold">{props.equipment.location}</span></p>
            <p className={`text-sm flex items-center}`}>
                {!props.equipment.isOperational && <AlertTriangle className="text-red-600 w-5 h-5 mr-1" />} 
                Operational: <span className="font-bold ml-1">{props.equipment.isOperational ? 'Yes' : 'No'}</span>
            </p>
            <br/>
            <p className="text-sm text-gray-600">Operator: <span className="font-bold">{props.equipment.operator}</span></p>
            <p className="text-sm text-gray-600">OOE: <span className="font-bold">{props.equipment.overallEquipmentEffectiveness}</span></p>
        </>
    )
}