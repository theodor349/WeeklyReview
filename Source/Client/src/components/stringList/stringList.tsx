"use client"

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
import { capitalize } from "@/lib/string"

export default function StringList({ form, entry, placeholder, selection }: StringListProps) {
  const { addItem, removeItem, resetNumber } = useStringList(form, entry);

  return (
    <>
      <FormLabel>{capitalize(entry)}</FormLabel>
      {form.getValues(entry).map((item: string, index: number) => (
        <FormField
          control={form.control}
          name={`${entry}[${index}]`}
          key={`${entry}_${index}`}
          render={({ field }) => (
            <FormItem>
              <FormControl>
                <Combobox 
                  placeholder={placeholder} 
                  form={form} 
                  resetNumber={resetNumber} 
                  entry={entry} 
                  index={index} 
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