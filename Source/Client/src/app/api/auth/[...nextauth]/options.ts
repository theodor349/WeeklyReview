import type { NextAuthOptions } from "next-auth";
import Google from "next-auth/providers/google";
import { v5 as uuidv5 } from "uuid";

const NAMESPACE = '6ba7b810-9dad-11d1-80b4-00c04fd430c8';

export const options: NextAuthOptions = {
  callbacks: {
    session: ({ session, token }) => {
      return {
        ...session,
        user: {
          ...session.user,
          id: uuidv5(token.sub, NAMESPACE ),
        },
      };
    }
  },
  providers: [
    Google({
      clientId: process.env.GOOGLE_ID!,
      clientSecret: process.env.GOOGLE_SECRET!,
    }),
  ],
}