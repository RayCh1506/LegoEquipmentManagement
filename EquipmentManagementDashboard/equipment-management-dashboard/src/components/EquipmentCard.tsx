import { Equipment } from "@/types/types";
import Link from "next/link";
import EquipmentData from "./EquipmentData";
import TimelineBar from "./TimelineBar";

export default function EquipmentCard(props: {equipment: Equipment}){
    return (
        <Link href={`/equipment/${props.equipment.id}`}>
            <div className="bg-gray-300 rounded-lg p-4 h-70 w-full place-items-center">
                <TimelineBar history={props.equipment.historicStates}/>
                <EquipmentData equipment={props.equipment}/>
            </div>
        </Link>
    )
}