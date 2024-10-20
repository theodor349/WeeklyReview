import { z } from "zod";

export const formSchema = z.object({
  date: z.object({
    date: z.date(),
    time: z.date(),
  }),
  activity: z.array(z.string()),
});

export type FormSchema = z.infer<typeof formSchema>;