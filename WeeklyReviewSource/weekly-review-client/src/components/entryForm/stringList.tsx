"use client"

import { useReducer } from "react"
import { z } from "zod"
import { zodResolver } from "@hookform/resolvers/zod"
import { useForm } from "react-hook-form"

import { Button } from "@/components/ui/button"
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"

interface Props {
  form: ReturnType<typeof useForm>
  entry: string
}

export default function StringList({ form, entry }: Props) {
  const [, forceUpdate] = useReducer(x => x + 1, 0);

  function addItem(){
    form.setValue(entry, [...form.getValues(entry), ""])
    forceUpdate();
  }

  function removeItem(){
    const length = form.getValues(entry).length
    console.log(form.getValues(entry))
    if(length === 1) 
      form.getValues(entry)[0] = "";
    else 
      form.setValue(entry, form.getValues(entry).slice(0, -1))
    console.log(form.getValues(entry))
    forceUpdate();
  }

  return (
    <>
      <FormLabel>{entry[0].toUpperCase() + entry.substring(1)}</FormLabel>
      {form.getValues(entry).map((item: string, index: number) => (
        <FormField
          control={form.control}
          name={`${entry}[${index}]`}
          key={`${entry}_${index}`}
          render={({ field }) => (
            <FormItem>
              <FormControl>
                <Input placeholder="Exercise: Running" {...field} />
              </FormControl>
              <FormMessage />
            </FormItem>
          )}
        />
      ))}
      <div className="flex flex-row justify-center gap-8">
        <Button className="w-24" size={"sm"} variant={"destructive"} onClick={(e) => { e.preventDefault(); removeItem() }}>Remove</Button>
        <Button className="w-24" size={"sm"} variant={"secondary"} onClick={(e) => { e.preventDefault(); addItem() }}>Add</Button>
      </div>
    </>
  )
}