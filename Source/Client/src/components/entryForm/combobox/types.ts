export type Item = string;
import { FormSchema } from "../client/formSchema";
import { UseFormReturn } from "react-hook-form";

export type ComboboxProps = {
  form: UseFormReturn<FormSchema>;
  index: number;
  placeholder: string;
  selection: Item[];
  resetNumber: number;
};