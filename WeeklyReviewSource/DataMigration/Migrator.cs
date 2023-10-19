using DataMigration.Models;
using DataMigration.Sql;
using System.Drawing;

public class Migrator : IMigrator
{
    private readonly IActivityAccess _activityAccess;
    private readonly IRecordAccess _recordAccess;
    private readonly IRecordTimeAccess _recordTimeAccess;

    public Migrator(IActivityAccess activityAccess, IRecordAccess recordAccess, IRecordTimeAccess recordTimeAccess)
    {
        _activityAccess = activityAccess;
        _recordAccess = recordAccess;
        _recordTimeAccess = recordTimeAccess;
    }

    public async Task Migrate()
    {
        SortedSet<DateTime> sortedrecordTimes;
        SortedDictionary<DateTime, List<int>> sortedrecords;
        SortedDictionary<int, string> sortedActivities;
        GetData(out sortedrecordTimes, out sortedrecords, out sortedActivities);

        var entries = MergeRecords(sortedrecordTimes, sortedrecords, sortedActivities);


    }

    private void GetData(out SortedSet<DateTime> sortedrecordTimes, out SortedDictionary<DateTime, List<int>> sortedrecords, out SortedDictionary<int, string> sortedActivities)
    {
        var recordTimes = _recordTimeAccess.GetAll();
        var records = _recordAccess.GetAll();
        var activities = _activityAccess.GetAll();

        sortedrecordTimes = new SortedSet<DateTime>();
        sortedrecords = new SortedDictionary<DateTime, List<int>>();
        sortedActivities = new SortedDictionary<int, string>();
        foreach (var r in recordTimes)
            sortedrecordTimes.Add(r.Date);
        foreach (var r in records)
        {
            if(sortedrecords.ContainsKey(r.RecordDate))
                sortedrecords[r.RecordDate].Add(r.ActivityId);
            else 
                sortedrecords.Add(r.RecordDate, new List<int>() { r.ActivityId });
        }
        foreach (var a in activities)
            sortedActivities.Add(a.Id, a.Name);
    }

    private List<EntryModel> MergeRecords(SortedSet<DateTime> sortedrecordTimes, SortedDictionary<DateTime, List<int>> sortedrecords, SortedDictionary<int, string> sortedActivities)
    {
        var entries = new List<EntryModel>();

        foreach (var date in sortedrecordTimes)
        {
            var e = new EntryModel();
            e.Date = date;
            foreach (var activityId in sortedrecords[date])
                e.Entries.Add(sortedActivities[activityId]);
            entries.Add(e);
        }

        return entries;
    }
}
