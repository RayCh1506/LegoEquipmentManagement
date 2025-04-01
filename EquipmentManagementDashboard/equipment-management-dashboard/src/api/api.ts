import { Equipment, EquipmentState } from "@/types/types";

const BASE_URL = "https://localhost:5001/Equipment"

export const fetchAllEquipment = async (): Promise<Equipment[]> => {
    const res = await fetch(`${BASE_URL}/GetAll`);
    if (!res.ok) await handleErrorResponse(res);
    return res.json();
};

export const fetchEquipment = async (id: string) => {
    const res = await fetch(`${BASE_URL}/${id}`);
    if (!res.ok) await handleErrorResponse(res);
    return res.json();
};

export const startEquipment = async (id: string, orderId: number | undefined) => {
    const res = await fetch(`${BASE_URL}/${id}/Start`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ orderId: orderId }),
    });
    if (!res.ok) await handleErrorResponse(res);
}

export const stopEquipment = async (id: string) => {
    const res = await fetch(`${BASE_URL}/${id}/Stop`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
    });
    if (!res.ok) await handleErrorResponse(res);
}

export const updateEquipmentState = async ({ id, newState, currentOrder }: { id: string; newState: EquipmentState, currentOrder: number | undefined }) => {
    const res = await fetch(`${BASE_URL}/${id}/UpdateState`, {
        method: "PATCH",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ newState, orderId: currentOrder }),
    });

    if (!res.ok) await handleErrorResponse(res);
};

const handleErrorResponse = async (res: Response) => {
    let errorMessage = "Unknown error occurred";
    try {
        const errorData = await res.json();
        console.error("Equipment Api Error:", errorData);
        errorMessage = errorData.detail || errorData.title || errorMessage;
    } catch (err) {
        console.error("Failed to parse error response", err);
    }
    throw new Error(errorMessage);
};
