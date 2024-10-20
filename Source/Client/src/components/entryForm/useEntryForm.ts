import { useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { formSchema, FormSchema } from "./formSchema";
import { addDays, roundMinutes, getMinutes } from "@/lib/date";
import { postEntry } from "./api";

export const useEntryForm = (initialSelection: string[]) => {
  const [postingEntry, setPostingEntry] = useState(false);
  const [localEntries, setLocalEntries] = useState<string[]>([]);

  const selection = [...initialSelection, ...localEntries];
  const initialDate = roundMinutes(new Date());

  const form = useForm<FormSchema>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      date: {
        date: initialDate,
        time: initialDate
      },
      activity: [""],
    },
  });

  const handleSubmit = async (values: FormSchema) => {
    const entries = [...values.activity.filter(Boolean)];
    const uniqueEntries = Array.from(new Set(entries));
    
    if (await confirmUnknownEntries(uniqueEntries)) {
      return;
    }

    setPostingEntry(true);
    try {
      const response = await postEntry(values, uniqueEntries);
      if (response.status !== 200) {
        throw new Error("Failed to add entry");
      }
      updateSelection(uniqueEntries);
    } catch (error) {
      alert("Failed to add entry");
    } finally {
      setPostingEntry(false);
    }
  };

  const confirmUnknownEntries = async (entries: string[]): Promise<boolean> => {
    const unknownEntries = entries.filter(entry => !selection.includes(entry));
    for (const entry of unknownEntries) {
      if (!window.confirm(`Do you want to add the entry: ${entry}`)) {
        return true;
      }
    }
    return false;
  };

  const updateSelection = (entries: string[]) => {
    const newEntries = entries.filter(entry => !selection.includes(entry));
    if (newEntries.length > 0) {
      setLocalEntries(prev => [...prev, ...newEntries]);
    }
  };

  const adjustTime = (minutes: number) => {
    const prevDate = form.getValues('date');
    const newTime = new Date(prevDate.time.getTime() + minutes * 60000);
    const newDate = shouldChangeDateByDay(prevDate.time, newTime)
      ? addDays(prevDate.date, minutes > 0 ? 1 : -1)
      : prevDate.date;
    form.setValue('date', { date: newDate, time: newTime });
  };

  const shouldChangeDateByDay = (oldTime: Date, newTime: Date): boolean => {
    return getMinutes(newTime) < getMinutes(oldTime);
  };

  const setTime = (timeString: string) => {
    const [hours, minutes] = timeString.split(':').map(Number);
    const prevDate = form.getValues('date');
    const newTime = new Date(prevDate.date);
    newTime.setHours(hours, minutes);
    form.setValue('date', { ...prevDate, time: roundMinutes(newTime) });
  }

  const resetTime = () => {
    const now = roundMinutes(new Date());
    form.setValue('date', { date: now, time: now });
  };

  return {
    form,
    handleSubmit,
    postingEntry,
    selection,
    adjustTime,
    setTime,
    resetTime,
  };
};