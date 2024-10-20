import { useReducer } from "react"

export const useStringList = (form: FormProps, entry: string) => {
  const [, forceUpdate] = useReducer(x => x + 1, 0);
  const [resetNumber, forceReset] = useReducer(x => x + 1, 0);

  const addItem = () => {
    form.setValue(entry, [...form.getValues(entry), ""])
    forceUpdate();
  }

  const removeItem = () => {
    const length = form.getValues(entry).length
    if (length === 1) {
      form.setValue(`${entry}[0]`, "")
      forceReset()
    } else {
      form.setValue(entry, form.getValues(entry).slice(0, -1))
    }
    forceUpdate();
  }

  return {
    addItem,
    removeItem,
    resetNumber
  }
}