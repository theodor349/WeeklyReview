import { getServerSession } from 'next-auth';
import { options } from '@/app/api/auth/[...nextauth]/options';

export default async function Home() {
  const session = await getServerSession(options);
  if(session){
    return (
      <main className="flex min-h-screen flex-col items-center justify-between p-24">
        <h1>Weekly Review</h1>
        <p>Signed in</p>
      </main>
    );
  }
  else {
    return (
      <main className="flex min-h-screen flex-col items-center justify-between p-24">
        <h1>Weekly Review</h1>
        <p>Not signed in</p>
      </main>
    );
  }
}
