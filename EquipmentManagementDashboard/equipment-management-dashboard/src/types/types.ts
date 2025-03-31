export type Equipment = {
    id: number;
    name: string;
    location: string;
    state: EquipmentState;
    isOperational: boolean;
    faultMessage: string;
    historicStates: EquipmentHistory[],
    currentOrder: number,
    assignedOrders: number[],
    overallEquipmentEffectiveness: number;
    operator: string;
}

export type EquipmentHistory = {
    fromState: EquipmentState;
    toState: EquipmentState;
    timeOfChange: string;
    orderId: number | undefined
}

export type EquipmentState = "RED" | "YELLOW" | "GREEN";