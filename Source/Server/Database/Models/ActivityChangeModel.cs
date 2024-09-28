namespace Database.Models;

public class ActivityChangeModel
{
    public int Id { get; set; }
    public ActivityModel Source { get; set; }
    public ActivityModel Destination { get; set; }
    public DateTime ChangeDate { get; set; }
    public Guid UserGuid { get; set; }

    public ActivityChangeModel()
    {

    }

    public ActivityChangeModel(ActivityModel source, ActivityModel destination, DateTime changeDate, Guid userGuid)
    {
        Source = source;
        Destination = destination;
        ChangeDate = changeDate;
        UserGuid = userGuid;
    }
}
