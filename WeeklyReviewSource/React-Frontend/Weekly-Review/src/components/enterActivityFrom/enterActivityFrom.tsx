"use client";

import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { date, z } from "zod";
import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button";
import {
  Form,
  FormControl,
  FormDescription,
  FormField,
  FormItem,
  FormLabel,
  FormMessage,
} from "@/components/ui/form";

import { Input } from "@/components/ui/input";
import { format } from "date-fns"
import { Calendar as CalendarIcon } from "lucide-react";
import { Calendar } from "@/components/ui/calendar";
import {
  Popover,
  PopoverContent,
  PopoverTrigger,
} from "@/components/ui/popover"
import { get } from "http";
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"

const formSchema = z.object({
  date: z.date(),
  activities: z.string().array(),
  socials: z.string().array(),
});

export function EnterActivityFrom() {
  // 1. Define your form.
  const form = useForm<z.infer<typeof formSchema>>({
    resolver: zodResolver(formSchema),
    defaultValues: {
      date: getDefaultDate(),
      activities: [""],
      socials: [""],
    },
  });

  // 2. Define a submit handler.
  function onSubmit(values: z.infer<typeof formSchema>) {
    // Do something with the form values.
    // ✅ This will be type-safe and validated.
    console.log(values);
  }
  
  function addActivity() {
    form.setValue("activities", [...form.getValues("activities"), ""])
  }

  function removeActivity() {
    const activities = form.getValues("activities")
    if(activities.length === 1) 
      return
    form.setValue("activities", activities.slice(0, activities.length - 1))
  }

  function addMinutes(minutes: number) {
    const date = form.getValues("date")
    form.setValue("date", new Date(date.getTime() + minutes * 60000))
  }

  function resetDate() {
    form.setValue("date", getDefaultDate())
  }

  function getDefaultDate(){
    var date = new Date();
    var minutes = date.getMinutes();
    var change = minutes % 15;
    date.setMinutes(minutes - change);
    return date;
  }

  function updateDate(date: Date) {
    const oldDate = form.getValues("date");
    date.setHours(oldDate.getHours());
    date.setMinutes(oldDate.getMinutes());
    form.setValue("date", date);
  }

  function addSocial() {
    form.setValue("socials", [...form.getValues("socials"), ""])
  }

  function removeSocial() {
    const socials = form.getValues("socials")
    if(socials.length === 1) 
      return
    form.setValue("socials", socials.slice(0, socials.length - 1))
  }

  return (
    <Card className="w-[350px]">
      <CardHeader>
        <CardTitle>Enter Activity</CardTitle>
        <CardDescription>Fill in the form below to enter an activity</CardDescription>
      </CardHeader>
      <CardContent>
        <Form {...form}>
          <form onSubmit={form.handleSubmit(onSubmit)} className="space-y-8">
            <FormField
              control={form.control}
              name="date"
              render={({ field: date }) => (
                <>
                  <FormItem>
                    <FormControl>
                      <Popover>
                        <PopoverTrigger asChild>
                          <Button
                            variant={"outline"}
                            className={cn(
                              "w-[280px] justify-start text-left font-normal",
                              !{...date} && "text-muted-foreground"
                            )}
                          >
                            <CalendarIcon className="mr-2 h-4 w-4" />
                            {date.value ? format(date.value, "MMM d, yyyy HH:mm") : "Select a date"}
                          </Button>
                        </PopoverTrigger>
                        <PopoverContent className="w-auto p-0">
                          <Calendar
                            mode="single"
                            selected={date.value}
                            onSelect={(date:Date) => {updateDate(date)}}
                            initialFocus
                          />
                        </PopoverContent>
                      </Popover>
                    </FormControl>
                    <FormMessage />
                  </FormItem>
                  <Button variant={"secondary"} onClick={() => addMinutes(-15)}>-15</Button>
                  <Button variant={"secondary"} onClick={() => resetDate()}>Reset</Button>
                  <Button variant={"secondary"} onClick={() => addMinutes(15)}>+15</Button>
                </>
              )}
            />
            <FormField
              control={form.control}
              name="activities"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Activities</FormLabel>
                  {field.value.map((activities, index) => (
                    <FormField
                      control={form.control}
                      name={`activities.${index}`}
                      render={({ field }) => (
                        <FormItem>
                          <FormControl>
                            <Input {...field} />
                          </FormControl>
                        </FormItem>
                      )}
                    />
                  ))}
                  <Button variant={"secondary"} onClick={() => removeActivity()}>Remove</Button>
                  <Button variant={"secondary"} onClick={() => addActivity()}>Add</Button>
                </FormItem>
              )}
            />
            <FormField
              control={form.control}
              name="socials"
              render={({ field }) => (
                <FormItem>
                  <FormLabel>Socials</FormLabel>
                  {field.value.map((socials, index) => (
                    <FormField
                      control={form.control}
                      name={`socials.${index}`}
                      render={({ field }) => (
                        <FormItem>
                          <FormControl>
                            <Input {...field} />
                          </FormControl>
                        </FormItem>
                      )}
                    />
                  ))}
                  <Button variant={"secondary"} onClick={() => removeSocial()}>Remove</Button>
                  <Button variant={"secondary"} onClick={() => addSocial()}>Add</Button>
                </FormItem>
              )}
            />
            <Button type="submit">Submit</Button>
          </form>
        </Form>
      </CardContent>
    </Card>
  );
}
