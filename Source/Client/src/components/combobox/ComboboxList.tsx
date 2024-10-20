import React from 'react';
import { cn } from "@/lib/utils";
import { Item } from './types';

type ComboboxListProps = {
  getMenuProps: () => React.HTMLAttributes<HTMLUListElement>;
  isOpen: boolean;
  items: Item[];
  getItemProps: (options: any) => any;
  highlightedIndex: number;
  selectedItem: Item | null;
};

export const ComboboxList: React.FC<ComboboxListProps> = ({
  getMenuProps,
  isOpen,
  items,
  getItemProps,
  highlightedIndex,
  selectedItem,
}) => (
  <ul
    className={cn(
      "absolute w-full max-w-xs md:max-w-sm bg-white mt-1 shadow-md max-h-80 overflow-scroll p-0 z-10",
      !(isOpen && items.length) && "hidden"
    )}
    {...getMenuProps()}
  >
    {isOpen &&
      items.map((item, index) => (
        <li
          className={cn(
            highlightedIndex === index && "bg-blue-300",
            selectedItem === item && "font-bold",
            "py-2 px-3 shadow-sm flex flex-col"
          )}
          key={item}
          {...getItemProps({ item, index })}
        >
          <span>{item}</span>
        </li>
      ))}
  </ul>
);