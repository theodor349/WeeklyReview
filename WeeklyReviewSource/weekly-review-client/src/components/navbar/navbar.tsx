"use client"

import Link from "next/link";
import React, { useState } from "react";
import { FaBars, FaTimes } from "react-icons/fa";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar"
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover"
import { signIn, signOut } from "next-auth/react"

const links = [
  {
    id: 1,
    link: "entry",
  },
  {
    id: 2,
    link: "review",
  },
];

export interface Props {
  avatarUrl: string;
}

export default function Navbar({ avatarUrl } : Props) {
  const [nav, setNav] = useState(false);

  return (
    <div className="flex justify-between items-center w-full bg-gray-900 p-2 fixed nav">
      <div>
        <h1 className="text-2xl text-white font-signature ml-2">
          <a
            className="link-underline link-underline-white"
            href="/"
          >
            Weekly Review
          </a>
        </h1>
      </div>

      <ul className="hidden md:flex items-center">
        {links.map(({ id, link }) => (
          <li
            key={id}
            className="nav-links px-4 cursor-pointer capitalize font-medium text-gray-300 hover:scale-105 hover:text-white duration-200 link-underline"
          >
            <Link href={link}>{link}</Link>
          </li>
        ))}
        <Popover>
          <PopoverTrigger>
            <Avatar>
              <AvatarImage src={avatarUrl} />
              <AvatarFallback>CN</AvatarFallback>
            </Avatar>
          </PopoverTrigger>
          <PopoverContent>
            <ul className="flex flex-col items-center">
              <li className="py-2">
                <button onClick={() => signOut()}>Sign Out</button>
              </li>
            </ul>
          </PopoverContent>
        </Popover>
      </ul>

{/* In case the viewport is too small */}

      <div
        onClick={() => setNav(!nav)}
        className="cursor-pointer pr-4 z-10 text-gray-300 md:hidden"
      >
        {nav ? <FaTimes size={30} /> : <FaBars size={30} />}
      </div>

      {nav && (
        <ul className="flex flex-col justify-center items-center absolute top-0 left-0 w-full h-screen bg-gray-900 text-gray-300">
          {links.map(({ id, link }) => (
            <li
              key={id}
              className="px-4 cursor-pointer capitalize py-6 text-4xl"
            >
              <Link onClick={() => setNav(!nav)} href={link}>
                {link}
              </Link>
            </li>
          ))}
          <li
            key={links.length + 1}
            className="px-4 cursor-pointer capitalize py-6 text-4xl"
          >
            <button onClick={() => signOut()}>Sign Out</button>
          </li>
        </ul>
      )}
    </div>
  )
}
