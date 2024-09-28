import { options } from '@/app/api/auth/[...nextauth]/options';
import { getServerSession } from 'next-auth';

export async function POST(req: Request) {
  const session = await getServerSession(options);
  let userId = session!.user?.id!
  const baseUrl = process.env.BACKEND_URL;

  if (userId == process.env.NEXT_USERID) {
    userId = process.env.DOTNET_USERID
  }

  const body = await req.json();
  const res = await fetch(`${baseUrl}/api/v1/entry/enter`, {
    method: "POST",
    body: JSON.stringify(body),
    headers: {
      "Content-Type": "application/json",
      "x-functions-key": process.env.FUNCTIONS_KEY || ""
    }
  })
  return new Response(JSON.stringify({message: "Entry added"}));
}