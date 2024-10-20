"use client";

import * as React from "react";
import { useCombobox } from 'downshift';
import { ComboboxProps, Item } from './types';
import { useComboboxItems } from './useComboboxItems';
import { ComboboxInput } from './ComboboxInput';
import { ComboboxList } from './ComboboxList';

const MAX_ITEMS = 24;

export function Combobox({ form, index, placeholder, selection, resetNumber }: ComboboxProps) {
  const [currentResetNumber, setCurrentResetNumber] = React.useState(0);
  const { items, setItems } = useComboboxItems(selection, MAX_ITEMS);

  const {
    isOpen,
    getToggleButtonProps,
    getMenuProps,
    getInputProps,
    highlightedIndex,
    getItemProps,
    selectedItem,
    selectItem,
  } = useCombobox({
    onInputValueChange({ inputValue }) {
      setItems(inputValue);
      form.setValue(`activity.${index}`, inputValue);
    },
    items,
    itemToString: (item: string | null) => item ?? '',
  });

  React.useEffect(() => {
    if (currentResetNumber !== resetNumber) {
      setCurrentResetNumber(resetNumber);
      selectItem(null);
    }
  }, [resetNumber, currentResetNumber, selectItem]);

  return (
    <div>
      <ComboboxInput
        getInputProps={getInputProps}
        getToggleButtonProps={getToggleButtonProps}
        isOpen={isOpen}
        placeholder={placeholder}
      />
      <ComboboxList
        getMenuProps={getMenuProps}
        isOpen={isOpen}
        items={items}
        getItemProps={getItemProps}
        highlightedIndex={highlightedIndex}
        selectedItem={selectedItem}
      />
    </div>
  );
}