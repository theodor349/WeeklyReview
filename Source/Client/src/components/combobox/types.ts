export type Item = string;
import { FormSchema } from "../entryForm/client/formSchema";
import { UseFormReturn } from "react-hook-form";

export type ComboboxProps = {
  onInputValueChange: (inputValue: string) => void;
  placeholder: string;
  selection: Item[];
  resetNumber: number;
};