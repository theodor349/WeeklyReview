
# System Definition 
The system is an activity tracking and analysis tool that allows users to log and categorize their activities. The system provides an easy and intuitive interface for starting and ending activities, such as Running, Attending University, or Taking the Train, as well as categorizing them for analysis and visualization. The system is capable of tracking all 24 hours of the day, allowing the user to log and analyze their activities at any time. The system presents the user’s activities in a clear and organized manner, allowing the user to analyze their time usage and gain insights into their behavior.

## FACTOR
### Functionality
- The user can start and stop activities at any time. Including in the past, the pressent and the future
- Activities can be 
  - Categorized
  - Visualized using different colors, which is user defined
  - Migrate/convert to other activites, e.g. converting `diner` to `dinner` 
- Possible to query all activities for metrics like 
  - Hours spent doing the activity
  - Number of times the activity has been logged
### Application Domain
The primary user will be the developer of the system.
However, it should also be possible for other users to use the system without much guidance.
### Conditions
Developed mostly by a single developer in his freetime, but with help from friends.
### Technology
- VPS, for hosting with limited resources 
- Web base UI for access anywhere
### Objects
See the section `Problem Domain/Classes`
### Responsibilities 
It must make sure that all activities which are recorded are safely stored and always avaliable for the user. 
Furthermore it must give correct numbers with regard to how much time and how many times an activity has been logged.


# Problem Domain
## Classes
- Entry
  - Contains time information and what activity is started
  - E.g. At 10:15 I was on call with the doctor, while out walking
- Activity
  - Is a label used to uniquely identify an activity 
  - E.g. Calling the doctor or walking 
- Category
  - Is a collection of activities
  - It is used to color the activity, e.g. sports are green and watching Netflix might be orange
  - E.g. Sports might include activities like: running, dancing, or swimmingd
- Query 
  - A selection of Activities and Categories that are interesting 
  - Used to query the database for interesting findings 
  - Example Queries
	  - How many hours have I spent dancing the last year? 
	  - Do I exercise at least 10 hours a week?
	  - How often do I call my parents?
	  - How much time have I spent calling my parents?
  - Input for a query could be a date range e.g. this year, or something even more complex like every evening this month
- User
  - Unique identifier used to seperate different users information
  - It is not modeled as it is just part of every object, and would cludder the analysis 

## Events 
- Activity 
  - Logged
    - It start a new entry and ends another 
  - Deleted
    - Only possible if no entries reference this activity 
  - Converted to a new Activity
- Category
  - Created
  - Edited Name
  - Edited Color
  - Edited Activates
  - Deleted
- Query
  - Created
  - Edited Name
  - Edited Activities  
  - Edited Categories 
  - Executed 
  - Deleted

## Event Table
| Event \ Object                         | Entry | Activity | Category | Query |
| --------------                         | :-:   | :-:      | :-:      | :-:   |
| Activity Logged                        | +     | *        |          |       |
| Activity Deleted                       |       | +        | *        | *     |
| Activity Converted to a new Activity   |       | *        | *        | *     |
| Category Created                       |       |          | +        |       |
| Category Edited Name                   |       |          | *        |       |
| Category Edited Color                  |       |          | *        |       |
| Category Edited Activates              |       |          | *        | *     |
| Category Deleted                       |       |          | +        | *     |
| Query Created                          |       |          |          | +     |
| Query Edited Name                      |       |          |          | *     |
| Query Edited Activities                |       |          |          | *     |
| Query Edited Categories                |       |          |          | *     |
| Executed                               |       |          |          | *     |
| Query Deleted                          |       |          |          | +     |

## Class Diagram
```mermaid
classDiagram
  Entry "*" *-- "*" Activity 
  Category "1" *-- "*" Activity
  Query "*" *-- "*" Activity
  Query "*" *-- "*" Category

  class Entry {
    + DateTime Start
    + DateTime End
    + TimeSpan GetDuration()
  }
  class Activity {
    + int Id
    + string Name
    + void ConvertToAnother()
  }
  class Category {
    + int Id
    + string Name
    + Color Color
    + void ChangeName()
    + void ChangeColor()
    + void AddActivity()
  }
  class Query {
    + int Id
    + string Name
    + TimeSpan GetTimeSpan()
    + int GetEntryCount()
    + void ChangeName()
    + void AddActivity()
    + void AddCategory()
  }
```