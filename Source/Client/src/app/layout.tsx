import type { Metadata } from "next";
import { Inter } from "next/font/google";
import "./globals.css";
import { getServerSession } from 'next-auth';
import { options } from '@/app/api/auth/[...nextauth]/options';
import Navbar from '@/components/navbar/navbar';

const inter = Inter({ subsets: ["latin"] });

export const metadata: Metadata = {
  title: "Weekly Review",
  description: "The app that provides insights into your activities from the past",
};

export default async function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  const session = await getServerSession(options);

  return (
    <html lang="en">
      <body className={inter.className + " bg-foreground"}>
        <Navbar avatarUrl={session?.user?.image ? session?.user?.image : ""} />
        {children}
      </body>
    </html>
  );
}
