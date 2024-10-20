import { format } from "date-fns";
import { FormSchema } from "./formSchema";

export const postEntry = async (values: FormSchema, entries: string[]) => {
  const adjustedTime = new Date(values.date.time.getTime() + values.date.time.getTimezoneOffset() * 60000);
  const data = {
    date: `${format(values.date.date, "yyyy-MM-dd")}T${format(adjustedTime, "HH:mm:ss")}Z`,
    entries,
  };
  const response = await fetch("/api/entry", {
    method: "POST",
    body: JSON.stringify(data),
  });
  return response.json();
};