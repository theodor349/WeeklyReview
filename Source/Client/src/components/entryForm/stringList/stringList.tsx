"use client"

import { UseFormReturn } from "react-hook-form";
import { Button } from "@/components/ui/button"
import {
  FormControl,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form"
import { Combobox } from "@/components/combobox/combobox"
import { useStringList } from "./useStringList"
import { FormSchema } from "../client/formSchema";

interface Props {
  form: UseFormReturn<FormSchema>;
  entry: string;
  placeholder: string;
  selection: string[];
}

export default function StringList({ form, placeholder, selection }: Props) {
  const { addItem, removeItem, resetNumber } = useStringList(form);

  return (
    <>
      <FormLabel>Activities</FormLabel>
      {form.getValues().activity.map((_: string, index: number) => (
        <FormField
          control={form.control}
          name={`activity.${index}`}
          key={`activity_${index}`}
          render={() => (
            <FormItem>
              <FormControl>
                <Combobox 
                  placeholder={placeholder} 
                  onInputValueChange={v => form.setValue(`activity.${index}`, v)} 
                  resetNumber={resetNumber} 
                  selection={selection} 
                />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
      ))}
      <div className="flex flex-row justify-center gap-8">
        <Button className="w-24" size="sm" variant="destructive" onClick={(e) => { e.preventDefault(); removeItem() }}>Remove</Button>
        <Button className="w-24" size="sm" variant="secondary" onClick={(e) => { e.preventDefault(); addItem() }}>Add</Button>
      </div>
    </>
  )
}