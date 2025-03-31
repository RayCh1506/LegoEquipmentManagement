"use client";

import Image from "next/image";
import Link from "next/link";
import { usePathname } from "next/navigation";
import logo from "../../public/LEGO_logo.svg.webp" 
import { useUser } from "@/providers/UserContextProvider";

const navLinks = [
    {
        href: "/",
        label: "Dashboard"
    },
    {
        href: "/login",
        label: "Login"
    }
]

export default function Header(){
    const pathName = usePathname(); 
    const {user} = useUser();
    return (
        <header className="flex justify-between items-center py-4 px-7 border-b bg-amber-400 mb-2">
            <div className="flex items-center gap-4">
                <Link href="/">
                    <Image
                        src={logo}
                        alt="logo"
                        className="w-[35px] h-[35px]" 
                        width={35} 
                        height={35} 
                    />
                </Link>
                <nav>
                    <ul className="flex gap-5 text-[14px]">
                        {navLinks.map((link) => (
                            <li key={link.href}>
                                <Link className={` ${pathName === link.href ? "text-zinc-900" : "text-zinc-400"}`} href={link.href}>
                                    {link.label}
                                </Link>
                            </li>
                        ))}
                    </ul>
                </nav>
            </div>

            <p className="text-sm text-gray-800">Current user: <span className="font-bold">{user || "None"}</span></p>

        </header>
    )
}