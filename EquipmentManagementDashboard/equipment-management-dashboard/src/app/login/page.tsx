"use client"

import { useUser } from "@/providers/UserContextProvider";

export default function LoginPage() 
{
    const {user, setUser} = useUser();

    return (
        <div className="place-items-center">
            <p>Login</p>  
            <select 
                className="px-3 py-2 border rounded-md text-black w-full max-w-[200px]"
                value={user} 
                onChange={(e) => setUser(e.target.value)}
            >
                <option value="">Not logged in</option>
                <option value="Worker">Worker</option>
                <option value="Supervisor">Supervisor</option>
            </select>
            <p className="mt-2">Current User: <span className="font-bold">{user || "Not logged in"}</span></p>
        </div>
    );
    
}