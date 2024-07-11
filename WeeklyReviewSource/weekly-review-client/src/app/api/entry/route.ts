import { options } from '@/app/api/auth/[...nextauth]/options';
import { getServerSession } from 'next-auth';

export async function POST(req: Request) {
  const session = await getServerSession(options);
  const userEmail = session!.user?.email!;

  const body = await req.json();
  const data = {
    email: userEmail,
    model: body
  }

  const res = await fetch("http://localhost:5197/api/v1/entry/enter", {
    method: "POST",
    body: JSON.stringify(data),
    headers: {
      "Content-Type": "application/json",
    }
  })
  console.log(res);
  return new Response(JSON.stringify({message: "Entry added"}));
}