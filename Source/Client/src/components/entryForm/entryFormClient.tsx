"use client"

import { useState } from "react"
import StringList from "@/components/entryForm/stringList"
import { useReducer } from "react"
import { cn } from "@/lib/utils"
import { date, z } from "zod"
import { format, set } from "date-fns"
import { zodResolver } from "@hookform/resolvers/zod"
import { useForm } from "react-hook-form"
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Button } from "@/components/ui/button"
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form"
import { Input } from "@/components/ui/input"
import { CalendarIcon } from "lucide-react"
import { Calendar } from "@/components/ui/calendar"
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover"

function addDays(date: Date, days: number) {
  date.setDate(date.getDate() + days);
  return date;
}

const formSchema = z.object({
  date: z.object({
    date: z.date(),
    time: z.date(),
  }),
  activity: z.array(z.string()),
  social: z.array(z.string()),
})

function roundMinutes(date: Date){
  date.setMinutes(Math.floor(date.getMinutes() / 15) * 15)
  date.setSeconds(0);
  date.setMilliseconds(0);
}

function getMinutes(date: Date){
  return date.getHours() * 60 + date.getMinutes()
}

interface Props {
  selection: string[]
}

export default function EntryFormClient({ selection }: Props) {
  const [calendarOpen, setCalendarOpen] = useState(false);

  var date = new Date();
  roundMinutes(date);

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      date: {
        date: date,
        time: date
      },
      activity: [""],
      social: [""],
    },
  })

  function onSubmit(values: z.infer<typeof formSchema>) {
    console.log(values)
    const entries = values.activity.filter(x => x !== "").concat(values.social.filter(x => x !== "").map(x => "Social: " + x));
    const uniqueEntries = Array.from(new Set(entries))
    values.date.time = new Date(values.date.time.getTime() + values.date.time.getTimezoneOffset() * 60000)
    const postData = async () => {
      const data = {
        date: format(values.date.date, "yyyy-MM-dd") + "T" + format(values.date.time, "HH:mm:ss") + "Z",
        entries: uniqueEntries,
      };

      const response = await fetch("/api/entry", {
        method: "POST",
        body: JSON.stringify(data),
      });
      return response.json();
    };
    postData().then((data) => {
      console.log(data.message);
    });

  }

  function addTime(minutes: number){
    var date = form.getValues('date');
    if(shouldSubtractOneDay()) {
      date.date = addDays(date.date, -1)
    }
    else if(shouldAddOneDay()){
      date.date = addDays(date.date, 1)
    }
    date.time = new Date(date.time.getTime() + minutes * 60000)
    form.setValue('date', date)

    function shouldAddOneDay(): boolean {
      return getMinutes(date.time) + minutes >= 1440
    }

    function shouldSubtractOneDay(): boolean {
      return getMinutes(date.time) + minutes < 0
    }
  }

  function overrideTime(time: string){
    const hours = parseInt(time.split(':')[0])
    const minutes = parseInt(time.split(':')[1])
    var date = form.getValues('date')
    console.log(date.time.getMinutes() != minutes, date.time.getMinutes(), minutes)

    date.time = new Date(date.date);
    date.time.setHours(hours);
    date.time.setMinutes(minutes);
    roundMinutes(date.time);
    form.setValue('date', date)
  }

  function resetTime(){
    var date = new Date();
    roundMinutes(date);
    form.setValue('date', 
    {
      date: date,
      time: date
    });
  }

  return (
    <Form {...form}>
      <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-2">
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
                          variant={"outline"}
                          className={cn(
                            "text-center font-normal w-full",
                            !field.value && "text-muted-foreground"
                          )}
                        >
                          {field.value ? (
                            format(field.value.date, "PPP")
                          ) : (
                            <span>Pick a date</span>
                          )}
                          <CalendarIcon className="ml-auto h-4 w-4 opacity-50" />
                        </Button>
                    </FormControl>
                  </PopoverTrigger>
                  <PopoverContent className="w-auto p-0" align="start">
                    <Calendar
                      mode="single"
                      selected={field.value.date}
                      onSelect={(date) => {form.setValue("date", {date: date === undefined ? field.value.date : date, time: field.value.time}); setCalendarOpen(false)}}
                      initialFocus
                    />
                  </PopoverContent>
                </Popover>
                <Input className="max-w-24" type="time"  onChange={(v) => overrideTime(v.target.value)} value={format(field.value.time, "HH:mm")}/>
              </div>
              <div className="flex flex-row justify-center gap-8">
                <Button size={"sm"} variant={"secondary"} onClick={(e) => { e.preventDefault(); addTime(-15) }}>-15</Button>
                <Button size={"sm"} variant={"secondary"} onClick={(e) => { e.preventDefault(); resetTime() }}>Reset</Button>
                <Button size={"sm"} variant={"secondary"} onClick={(e) => { e.preventDefault(); addTime(15) }}>+15</Button>
              </div>
              <FormMessage />
            </FormItem>
          )}
        />
        <StringList form={form as unknown as { setValue: (name: string, value: any) => void; getValues: (name: string) => any; control: any; }} entry="activity" placeholder="Exercise: Running" selection={selection} />
        {/* <StringList form={form} entry="social" placeholder="John Doe" selection={selection} /> */}
        <div className="flex justify-center">
          <Button className="w-32" size={"sm"} type="submit">Submit</Button>
        </div>
      </form>
    </Form>
  )
}