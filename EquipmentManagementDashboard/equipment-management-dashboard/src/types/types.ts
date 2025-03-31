export type Equipment = {
    id: number;
    generalInformation: GeneralInformation,
    orderInformation: OrderInformation,
    operationalInformation: OperationalInformation
    historicStates: EquipmentHistory[],
}

export type EquipmentHistory = {
    fromState: EquipmentState;
    toState: EquipmentState;
    timeOfChange: string;
    orderId: number | undefined
}

export type GeneralInformation = {
    name: string;
    location: string;
    state: EquipmentState;
}

export type OperationalInformation = {
    isOperational: boolean;
    faultMessage: string;
    overallEquipmentEffectiveness: number;
    operator: string;
}

export type OrderInformation = {
    currentOrder: number,
    assignedOrders: number[],
}

export type EquipmentState = "RED" | "YELLOW" | "GREEN";