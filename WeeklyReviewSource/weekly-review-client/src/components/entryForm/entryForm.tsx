import EntryFormClient from "@/components/entryForm/entryFormClient"
import {
  Card,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"

const selection = [
  "Go for a walk",
  "Read a book",
  "Watch a movie",
  "Cook a meal",
  "Bake cookies",
  "Do a puzzle",
  "Learn a new skill",
  "Practice yoga",
  "Meditate",
  "Call a friend or family member",
  "Write in a journal",
  "Clean the house",
  "Organize your closet",
  "Play a game",
  "Go for a bike ride",
  "Listen to music",
  "Take a nap",
  "Do some gardening",
  "Work on a creative project"
  ]

export default function EntryForm() {

  return (
    <Card>
      <CardHeader>
        <CardTitle>Enter Entry</CardTitle>
      </CardHeader>
      <CardContent className="max-w-md">
        <EntryFormClient selection={selection}/>
      </CardContent>
    </Card>
  )
}