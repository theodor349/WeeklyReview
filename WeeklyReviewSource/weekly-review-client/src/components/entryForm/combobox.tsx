"use client";

import * as React from "react";
import { Check, ChevronsUpDown } from "lucide-react";
import { cn } from "@/lib/utils";
import { ControllerRenderProps, FieldValues } from "react-hook-form";
import { Input } from "@/components/ui/input";
import {
  Command,
  CommandEmpty,
  CommandGroup,
  CommandInput,
  CommandItem,
} from "@/components/ui/command";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover";
import { CommandList } from "cmdk";

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
};

export default function Combobox(
  { form, entry, index, placeholder, selection }: Props,
) {
  const [open, setOpen] = React.useState(false);
  const [inputValue, setInputValue] = React.useState("");

  return (
    <>
      <Command>
        <CommandInput placeholder={placeholder} onFocus={() => {setOpen(true)}}/>
        <Popover open={open} onOpenChange={setOpen}>
          <PopoverTrigger>
          </PopoverTrigger>
          <PopoverContent className="min-w-32">
            <CommandGroup>
              <CommandList>
                {selection.map((value) => (
                  <CommandItem
                    key={value}
                    value={value}
                    onSelect={(currentValue) => {
                      setInputValue(currentValue);
                      form.setValue(`${entry}[${index}]`, currentValue);
                      setOpen(false);
                    }}
                  >
                    {value}
                  </CommandItem>
                ))}
              </CommandList>
            </CommandGroup>
          </PopoverContent>
        </Popover>
      </Command>
    </>
  );
}
