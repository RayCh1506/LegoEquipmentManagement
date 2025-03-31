import { Equipment, EquipmentState } from "@/types/types";

const BASE_URL = "https://localhost:7031/Equipment"

export const fetchAllEquipment = async (): Promise<Equipment[]> => {
    const res = await fetch(`${BASE_URL}/GetAll`);
    if (!res.ok) throw new Error('Failed to fetch all equipment information');
    return res.json();
};

export const fetchEquipment = async (id: string) => {
    const res = await fetch(`${BASE_URL}/${id}`);
    if (!res.ok) throw new Error('Failed to fetch equipment information');
    return res.json();
};

export const startEquipment = async (id: string, orderId: number | undefined) => {
    const res = await fetch(`${BASE_URL}/${id}/Start`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ orderId: orderId }),
    });
    if (!res.ok) throw new Error("Failed to start equipment");
}

export const stopEquipment = async (id: string) => {
    const res = await fetch(`${BASE_URL}/${id}/Stop`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
    });
    if (!res.ok) throw new Error("Failed to stop equipment");
}

export const updateEquipmentState = async ({ id, newState, currentOrder }: { id: string; newState: EquipmentState, currentOrder: number | undefined }) => {
    const res = await fetch(`${BASE_URL}/${id}/UpdateState`, {
        method: "PATCH",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ newState: newState, orderId: currentOrder }),
    });
    if (!res.ok) throw new Error("Failed to update equipment state");
};
