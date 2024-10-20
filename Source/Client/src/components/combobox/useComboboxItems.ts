import { useState, useCallback } from 'react';
import { Item } from './types';

const getFilter = (inputValue: string) => {
  const lowerCasedInputValue = inputValue.toLowerCase();
  return (item: Item) => !inputValue || item.toLowerCase().includes(lowerCasedInputValue);
};

export const useComboboxItems = (selection: Item[], maxItems: number) => {
  const [items, setItems] = useState<Item[]>(selection.slice(0, maxItems));

  const updateItems = useCallback((inputValue: string | undefined) => {
    setItems(selection.filter(getFilter(inputValue ?? '')).slice(0, maxItems));
  }, [selection, maxItems]);

  return { items, setItems: updateItems };
};