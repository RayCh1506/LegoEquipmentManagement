"use client";

import { createContext, useContext, useEffect, useState } from "react";

const UserContext = createContext<{ user: string; setUser: (user: string) => void } | undefined>(undefined);

export function useUser() {
    const context = useContext(UserContext);

    if (!context) {
        throw new Error("useUser must be used within a UserContextProvider");
    }

    return context;
}

export function UserContextProvider({ children }: { children: React.ReactNode }) {
    const [user, setUser] = useState<string>("");
    
    useEffect(() => {
        setUser(localStorage.getItem("user") ?? "")
    }, [])

    return (
        <UserContext.Provider value={{ user, setUser }}>
            {children}
        </UserContext.Provider>
    );
}