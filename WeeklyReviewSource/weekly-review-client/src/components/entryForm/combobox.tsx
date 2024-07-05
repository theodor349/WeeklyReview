"use client"

import * as React from "react"
import { Check, ChevronsUpDown } from "lucide-react"
import { cn } from "@/lib/utils"
import { ControllerRenderProps, FieldValues } from 'react-hook-form'
import { Input } from "@/components/ui/input"
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
} from "@/components/ui/command"
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover"
import { CommandList } from "cmdk"


type Props = {
  form: {
    setValue: (name: string, value: any) => void;
    getValues: (name: string) => any;
    control: any;
  };
  entry: string;
  index: number;
  placeholder: string;
  selection: string[];
}

export default function Combobox({ form, entry, index, placeholder, selection }: Props) {
  const [open, setOpen] = React.useState(false)
  const [value, setValue] = React.useState("")

  return (
    <>
      <Popover open={open} onOpenChange={setOpen}>
        <PopoverTrigger asChild>
          <Command>
            <CommandInput placeholder={placeholder} />
          </Command>
        </PopoverTrigger>
        <PopoverContent className="w-[200px] p-0">
          <Command>
            <CommandInput placeholder={placeholder} />
            <CommandEmpty>No results...</CommandEmpty>
            <CommandGroup>
              <CommandList>
              {selection.map((value) => (
                <CommandItem
                  key={value}
                  value={value}
                  onSelect={(currentValue) => {
                    setValue(currentValue === value ? "" : currentValue);
                    setOpen(false);
                    // form.setValue(`${entry}[${index}]`, currentValue === value ? "" : currentValue)
                  }}
                >
                  {value}
                </CommandItem>
              ))}
              </CommandList>
            </CommandGroup>
          </Command>
        </PopoverContent>
      </Popover>
    </>
  )
}