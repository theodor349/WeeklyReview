import EntryFormClient from "@/components/entryForm/entryFormClient"
import { options } from '@/app/api/auth/[...nextauth]/options';
import { getServerSession } from 'next-auth';
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"

export default async function EntryForm() {
  const session = await getServerSession(options);
  const userId = session!.user?.id!

  var selection: string[] = [];
  await fetch(`http://localhost:7151/api/v1/user/${userId}/activities`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    }
  }).then(response => response.json()).then(data => selection = data.map((item: any) => item.name));
  
  return (
    <Card>
      <CardHeader>
        <CardTitle>Enter Entry</CardTitle>
      </CardHeader>
      <CardContent className="max-w-md">
        <EntryFormClient selection={selection}/>
      </CardContent>
    </Card>
  )
}