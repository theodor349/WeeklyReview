"use client";

import { useState } from "react";
import { useForm } from "react-hook-form";
import { zodResolver } from "@hookform/resolvers/zod";
import { z } from "zod";
import { format, isValid } from "date-fns";
import { Button } from "@/components/ui/button";
import { Form, FormControl, FormField, FormItem, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { CalendarIcon } from "lucide-react";
import { Calendar } from "@/components/ui/calendar";
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover";
import StringList from "@/components/entryForm/stringList";
import { cn } from "@/lib/utils";
import { addDays, roundMinutes, getMinutes } from "@/lib/date";

const formSchema = z.object({
  date: z.object({
    date: z.date(),
    time: z.date(),
  }),
  activity: z.array(z.string()),
  social: z.array(z.string()),
});

type FormSchema = z.infer<typeof formSchema>;

interface Props {
  selection: string[];
}

export default function EntryFormClient({ selection: initialSelection }: Props) {
  const [calendarOpen, setCalendarOpen] = useState(false);
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
      social: [""],
    },
  });

  const handleSubmit = async (values: FormSchema) => {
    const entries = [...values.activity.filter(Boolean), ...values.social.filter(Boolean).map(x => `Social: ${x}`)];
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

  const postEntry = async (values: FormSchema, entries: string[]) => {
    const adjustedTime = new Date(values.date.time.getTime() + values.date.time.getTimezoneOffset() * 60000);
    const data = {
      date: `${format(values.date.date, "yyyy-MM-dd")}T${format(adjustedTime, "HH:mm:ss")}Z`,
      entries,
    };
    const response = await fetch("/api/entry", {
      method: "POST",
      body: JSON.stringify(data),
    });
    return response.json();
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

  const formatDate = (date: Date | null): string => {
    if (!date || !isValid(date)) {
      return 'Invalid date';
    }
    return format(date, "PPP");
  };

  const formatTime = (date: Date | null): string => {
    if (!date || !isValid(date)) {
      return '';
    }
    return format(date, "HH:mm");
  };

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(handleSubmit)} className="space-y-2">
        <FormField
          control={form.control}
          name="date"
          render={({ field }) => (
            <FormItem className="flex flex-col">
              <div className="flex flex-row gap-2">
                <Popover open={calendarOpen} onOpenChange={setCalendarOpen}>
                  <PopoverTrigger asChild>
                    <FormControl>
                      <Button
                        variant="outline"
                        className={cn(
                          "text-center font-normal w-full",
                          !field.value.date && "text-muted-foreground"
                        )}
                      >
                        {field.value.date ? formatDate(field.value.date) : <span>Pick a date</span>}
                        <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                      </Button>
                    </FormControl>
                  </PopoverTrigger>
                  <PopoverContent className="w-auto p-0" align="start">
                    <Calendar
                      mode="single"
                      selected={field.value.date}
                      onSelect={(date) => {
                        if (date) {
                          form.setValue("date", { ...field.value, date });
                          setCalendarOpen(false);
                        }
                      }}
                      initialFocus
                    />
                  </PopoverContent>
                </Popover>
                <Input
                  className="max-w-24"
                  type="time"
                  onChange={(e) => setTime(e.target.value)}
                  value={formatTime(field.value.time)}
                />
              </div>
              <div className="flex flex-row justify-center gap-8">
                <Button size="sm" variant="secondary" onClick={(e) => { e.preventDefault(); adjustTime(-15); }}>-15</Button>
                <Button size="sm" variant="secondary" onClick={(e) => { e.preventDefault(); resetTime(); }}>Reset</Button>
                <Button size="sm" variant="secondary" onClick={(e) => { e.preventDefault(); adjustTime(15); }}>+15</Button>
              </div>
              <FormMessage />
            </FormItem>
          )}
        />
        <StringList
          form={form}
          entry="activity"
          placeholder="Exercise: Running"
          selection={selection}
        />
        <div className="flex justify-center">
          <Button className="w-32" size="sm" type="submit" disabled={postingEntry}>
            Submit
          </Button>
        </div>
      </form>
    </Form>
  );
}