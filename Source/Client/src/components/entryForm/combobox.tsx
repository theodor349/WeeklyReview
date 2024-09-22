"use client";

import {useCombobox} from 'downshift'
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
  resetNumber: number
};

function getFilter(inputValue: string) {
  const lowerCasedInputValue = inputValue.toLowerCase()

  return function filter(inputValue: string) {
    return (
      !inputValue ||
      inputValue.toLowerCase().includes(lowerCasedInputValue)
    )
  }
}

const MAX_ITEMS = 24;
export default function Combobox(
  { form, entry, index, placeholder, selection, resetNumber }: Props,
) {
  const [currentResetNumber, setcurrentResetNumber] = React.useState(0)
  const [items, setItems] = React.useState(selection.slice(0, MAX_ITEMS))
  const {
    isOpen,
    getToggleButtonProps,
    getLabelProps,
    getMenuProps,
    getInputProps,
    highlightedIndex,
    getItemProps,
    selectedItem,
    selectItem,
  } = useCombobox({
    onInputValueChange({inputValue}) {
      setItems(selection.filter(getFilter(inputValue)).slice(0, MAX_ITEMS))
      form.setValue(`${entry}[${index}]`, inputValue);
    },
    items,
    itemToString(item) {
      return item ? item : ''
    },
  })

  function reset() {
    setcurrentResetNumber(resetNumber);
    selectItem(null);
  }
  
  if(currentResetNumber != resetNumber){
    reset();
  }


  return (
    <div>
      <div className="flex flex-col gap-1">
        <div className="flex shadow-sm bg-white gap-0.5">
          <input
            placeholder={placeholder}
            className="w-full p-1.5"
            {...getInputProps()}
          />
          <button
            aria-label="toggle menu"
            className="px-2"
            type="button"
            {...getToggleButtonProps()}
          >
            {isOpen ? <>&#8593;</> : <>&#8595;</>}
          </button>
        </div>
      </div>
      <ul
        className={`absolute w-full max-w-xs md:max-w-sm bg-white mt-1 shadow-md max-h-80 overflow-scroll p-0 z-10 ${
          !(isOpen && items.length) && 'hidden'
        }`}
        {...getMenuProps()}
      >
        {isOpen &&
          items.map((item, index) => (
            <li
              className={cn(
                highlightedIndex === index && 'bg-blue-300',
                selectedItem === item && 'font-bold',
                'py-2 px-3 shadow-sm flex flex-col',
              )}
              key={item}
              {...getItemProps({item, index})}
            >
              <span>{item}</span>
            </li>
          ))}
      </ul>
    </div>
  )
}
