"use client"

import StringList from "@/components/entryForm/stringList"
import { useReducer } from "react"
import { cn } from "@/lib/utils"
import { z } from "zod"
import { format } from "date-fns"
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

const formSchema = z.object({
  date: z.date(),
  activity: z.array(z.string()),
  social: z.array(z.string()),
})

function roundMinutes(date: Date){
  date.setMinutes(Math.floor(date.getMinutes() / 15) * 15)
  date.setSeconds(0);
  date.setMilliseconds(0);
}

export default function EntryForm() {
  const [, forceUpdate] = useReducer(x => x + 1, 0);

  var date = new Date();
  roundMinutes(date);

  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      date: date,
      activity: [""],
      social: [""],
    },
  })

  function onSubmit(values: z.infer<typeof formSchema>) {
    console.log(values)
  }

  function addTime(minutes: number){
    form.setValue('date', new Date(form.getValues('date').getTime() + minutes * 60000))
  }

  function overrideTime(time: string){
    const hours = parseInt(time.split(':')[0])
    const minutes = parseInt(time.split(':')[1])
    var date = new Date(form.getValues('date').setHours(hours, minutes))
    roundMinutes(date);
    form.setValue('date', date)
  }

  function resetTime(){
    var date = new Date();
    roundMinutes(date);
    form.setValue('date', date);
  }

  function addActivity(){
    console.log("Add Activity")
    console.log(form.getValues('activity'))
    form.setValue('activity', [...form.getValues('activity'), ""])
    console.log(form.getValues('activity'))
    forceUpdate();
  }

  function removeActivity(){
    console.log("Remove Activity")
    const length = form.getValues('activity').length
    if(length === 1) 
      form.setValue('activity', [""])
    else 
      form.setValue('activity', form.getValues('activity').slice(0, -1))
    forceUpdate();
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>Enter Entry</CardTitle>
      </CardHeader>
      <CardContent className="max-w-md">
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-2">
          <FormField
            control={form.control}
            name="date"
            render={({ field }) => (
              <FormItem className="flex flex-col">
                <div className="flex flex-row gap-2">
                  <Popover>
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
                              format(field.value, "PPP")
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
                        selected={field.value}
                        onSelect={field.onChange}
                        initialFocus
                      />
                    </PopoverContent>
                  </Popover>
                  <Input className="max-w-24" type="time" onChange={(v) => overrideTime(v.target.value)} value={format(field.value, "HH:mm")}/>
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
          <StringList form={form} entry="activity" placeholder="Exercise: Running" />
          <StringList form={form} entry="social" placeholder="John Doe" />
          <div className="flex justify-center">
            <Button className="w-32" size={"sm"} type="submit">Submit</Button>
          </div>
        </form>
      </Form>
      </CardContent>
    </Card>
  )
}