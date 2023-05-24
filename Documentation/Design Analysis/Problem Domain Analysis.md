
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
See the section `Problem Domain/Objects`
### Responsibilities 
It must make sure that all activities which are recorded are safely stored and always avaliable for the user. 
Furthermore it must give correct numbers with regard to how much time and how many times an activity has been logged.


# Problem Domain
## Objects
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

## Events 
- Activity 
  - Started
  - Ended
  - Created
  - Edited Name
  - Deleted
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
  - Edited Activities and Categories 
  - Executed 
  - Deleted
- User
  - Signed up
  - Deleted

## Event Table
| Event \ Object                         | Entry | Activity | Category | Query | User |
| --------------                         | :-:   | :-:      | :-:      | :-:   | :-:  |
| Activity Started                       | x     | x        |          |       | x    |
| Activity Ended                         | x     | x        |          |       |      |
| Activity Created                       |       | x        |          |       | x    |
| Activity Edited name                   |       | x        |          |       |      |
| Activity Deleted                       |       | x        | x        | x     | x    |
| Activity Converted to a new Activity   |       | x        | x        | x     | x    |
| Category Created                       |       |          | x        |       | x    |
| Category Edited Name                   |       |          | x        |       |      |
| Category Edited Color                  |       |          | x        |       |      |
| Category Edited Activates              |       |          | x        | x     |      |
| Category Deleted                       |       |          | x        | x     | x    |
| Query Createed                         |       |          |          | x     | x    |
| Query Edited Name                      |       |          |          | x     |      |
| Query Edited Activities and Categories |       |          |          | x     |      |
| Executed                               |       |          |          | x     |      |
| Query Deleted                          |       |          |          | x     | x    |
| User Signed up                         |       |          |          |       | x    |
| User Deleted                           | x     | x        | x        | x     | x    |