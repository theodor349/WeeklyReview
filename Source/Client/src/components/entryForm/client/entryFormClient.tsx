"use client";

import { useState } from "react";
import { Button } from "@/components/ui/button";
import { Form, FormControl, FormField, FormItem, FormMessage } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { CalendarIcon } from "lucide-react";
import { Calendar } from "@/components/ui/calendar";
import { Popover, PopoverContent, PopoverTrigger } from "@/components/ui/popover";
import StringList from "@/components/entryForm/stringList/stringList";
import { cn } from "@/lib/utils";
import { useEntryForm } from "./useEntryForm";
import { formatDate, formatTime } from "@/lib/date";

interface Props {
  selection: string[];
}

export default function EntryFormClient({ selection: initialSelection }: Props) {
  const [calendarOpen, setCalendarOpen] = useState(false);
  const {
    form,
    handleSubmit,
    postingEntry,
    selection,
    adjustTime,
    setTime,
    resetTime,
  } = useEntryForm(initialSelection);

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