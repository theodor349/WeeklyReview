"use client"

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
    date = new Date(form.getValues('date').setHours(hours, minutes))
    roundMinutes(date);
    form.setValue('date', date)
  }

  function resetTime(){
    form.setValue('date', new Date())
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>Enter Entry</CardTitle>
      </CardHeader>
      <CardContent className="max-w-md">
      <Form {...form}>
        <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
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
                <Button variant={"secondary"} onClick={(e) => { e.preventDefault(); addTime(-15) }}>-15</Button>
                <Button variant={"secondary"} onClick={(e) => { e.preventDefault(); resetTime() }}>Reset</Button>
                <Button variant={"secondary"} onClick={(e) => { e.preventDefault(); addTime(15) }}>+15</Button>
              </div>
              <FormMessage />
            </FormItem>
          )}
        />
          <FormField
            control={form.control}
            name="activity[0]"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Activity</FormLabel>
                <FormControl>
                  <Input placeholder="shadcn" {...field} />
                </FormControl>
                <div className="flex flex-row justify-center gap-8">
                  <Button className="w-32" variant={"destructive"}>Remove</Button>
                  <Button className="w-32" variant={"secondary"}>Add</Button>
                </div>
                <FormMessage />
              </FormItem>
            )}
          />
          <FormField
            control={form.control}
            name="social[0]"
            render={({ field }) => (
              <FormItem>
                <FormLabel>Social</FormLabel>
                <FormControl>
                  <Input placeholder="shadcn" {...field} />
                </FormControl>
                <div className="flex flex-row justify-center gap-8">
                  <Button className="w-32" variant={"destructive"}>Remove</Button>
                  <Button className="w-32" variant={"secondary"}>Add</Button>
                </div>
                <FormMessage />
              </FormItem>
            )}
          />

          <Button type="submit">Submit</Button>
        </form>
      </Form>
      </CardContent>
    </Card>
  )
}