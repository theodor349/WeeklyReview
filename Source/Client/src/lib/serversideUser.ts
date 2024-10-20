import { getServerSession } from 'next-auth';
import { options } from '@/app/api/auth/[...nextauth]/options';

export async function getUserId() {
  const session = await getServerSession(options);
  let userId = session!.user?.id!

  if (userId == process.env.NEXT_USERID) {
    userId = process.env.DOTNET_USERID
  }
  return userId
}

