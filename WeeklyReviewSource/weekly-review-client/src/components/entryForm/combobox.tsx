import React from 'react'
import { ControllerRenderProps, FieldValues } from 'react-hook-form'
import { Input } from "@/components/ui/input"

type Props = {
  field: ControllerRenderProps<FieldValues, `${string}[${number}]`>,
  placeholder: string,
  selection: string[],
}

export default function Combobox({ field, placeholder, selection }: Props) {
  return (
    <>
      <Input placeholder={placeholder} {...field} />
    </>
  )
}