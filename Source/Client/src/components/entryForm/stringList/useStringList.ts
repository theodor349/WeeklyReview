import { useReducer } from "react"
import { FormSchema } from "../client/formSchema";
import { UseFormReturn } from "react-hook-form";

export const useStringList = (form: UseFormReturn<FormSchema>) => {
  const [, forceUpdate] = useReducer(x => x + 1, 0);
  const [resetNumber, forceReset] = useReducer(x => x + 1, 0);

  const addItem = () => {
    form.setValue("activity", [...form.getValues().activity, ""])
    forceUpdate();
  }

  const removeItem = () => {
    const length = form.getValues().activity.length
    if (length === 1) {
      form.setValue(`activity.${0}`, "");
      forceReset()
    } else {
      form.setValue("activity", form.getValues().activity.slice(0, -1))
    }
    forceUpdate();
  }

  return {
    addItem,
    removeItem,
    resetNumber
  }
}