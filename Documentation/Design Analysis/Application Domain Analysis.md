# Application Domain Analysis 
## Actors 
- User, who wants to know more avout what he/she is doing with their time.

## Use Cases 
### Log an Activity
The user is about to get on their bike, and would therefor log that they are starting the bike activity.
When the user gets to their destination, they would like to log that they are now at the cinema 
### Log multiple activities 
The user just woke up and is going eat breakfast while watching some tv.
### Log Activity with new Activity and Category
The user logs an unknown activity with an unknown category, and would like the system to automatically create a new activity and category, as well as linking the two.
### Convert an activity 
The user noticies that they have entered an two activities "biking" and "cycling", and would like to convert the cycling activities into biking activities. 
### Spelling mistake in an activity 
The user noticies that they have misspelled the title of a move, and would like to correct that.
### Create new Categoey
The user wants to categorize some activites into a new category, and therefor hey make a new category.
Then they selects all the activities which should be part of that category.
### Create new Query 
The user would like to know about how much exercise they do, so they make a new query and selects all the activities and categories which has something to do with exercise e.g. the category exercise and the activie biking from transportation. 
### Analyse sleep habbits
The user wants to know how much sleep they have gotten thoughout the last week.
Or how their amount of sleep has changed over the last few months.
### Analyse who they call 
The user wants to know how many times they have called a specific person or group of people.
### Analyse how often they do an activity 
The user has a goal of running atleast five times a week, and would like to see if they have meet that goal.
### Share time usage 
The user has a webpage where they want to share how much time they have used on a project.

## Fucntions
| FunctionNames         | Complexity | Type    | Description |
| ---                   | :-:        | :-:     | ---         |
| Start Entry           | Medium     | Update  | Creates a new entry and reference relevant activities |
| End Entry             | Simple     | Update  | Updates the end time and duration |
| Log Activity          | Complex    | Update  | Delete entry at entry date, start new entry, end earlier entry |
| Create Activity       | Medium     | Update  |             |
| Update Activity name  | Simple     | Update  |             |
| Convert Activity      | Complex    | Update  | Update all reference to that activity to the new one, and then delete the old activity |
| Delete Activity       | Simple     | Update  |             |
| Create Category       | Medium     | Update  |             |
| Update Category       | Simple     | Update  |             |
| Delete Category       | Simple     | Update  |             |
| Create Query          | Medium     | Update  |             |
| Update Query          | Simple     | Update  |             |
| Delete Query          | Simple     | Update  |             |
| Execute Query         | Complex    | Read    | First find all activity ids which are part of the query, then find all the entries that reference those ids within the time period, and then calculate how much time was spent and count how many times it has been logged |


- Simple, means it just modifies a single object
- Medium, means it creates/deletes a single object or modifies multple objects
- Complex, means it creates/deletes multiple objects 