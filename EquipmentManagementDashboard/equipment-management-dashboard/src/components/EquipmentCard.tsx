import { Equipment } from "@/types/types";
import Link from "next/link";
import EquipmentData from "./EquipmentData";
import TimelineBar from "./TimelineBar";

export default function EquipmentCard(props: {equipment: Equipment}){
    return (
        <Link href={`/equipment/${props.equipment.id}`}>
            <div className="bg-gray-300 rounded-lg p-4 h-60 w-10/12 place-items-center">
                <TimelineBar/>
                <EquipmentData equipment={props.equipment}/>
            </div>
        </Link>
    )
}