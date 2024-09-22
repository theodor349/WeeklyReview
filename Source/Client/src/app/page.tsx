import { getServerSession } from 'next-auth';
import { options } from '@/app/api/auth/[...nextauth]/options';
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import EntryForm from '@/components/entryForm/entryForm';

export default async function Home() {

  return (
    <main className="flex flex-col min-h-screen h-full">
        <div className='flex flex-col items-center justify-between w-full h-64 md:mt-14 mt-12'>
          <div className='w-full p-2'>
            <EntryForm/>
          </div>
          <div className='w-full p-2'>
            <Card> 
              <CardHeader>
                <CardTitle>Weekly Review</CardTitle>
              </CardHeader>
              <CardContent>
                <CardDescription>
                  The app that provides insights into your activities from the past
                </CardDescription>
              </CardContent>
              <CardFooter>
                <button className='btn-primary'>Get Started</button>
              </CardFooter>
            </Card>
          </div>
        </div>
    </main>
  );
}
