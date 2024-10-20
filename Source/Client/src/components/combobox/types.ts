export type Item = string;

export type ComboboxProps = {
  form: {
    setValue: (name: string, value: any) => void;
    getValues: (name: string) => any;
    control: any;
  };
  entry: string;
  index: number;
  placeholder: string;
  selection: Item[];
  resetNumber: number;
};