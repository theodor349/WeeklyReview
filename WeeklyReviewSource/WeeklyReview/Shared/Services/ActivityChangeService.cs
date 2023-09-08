using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;
using WeeklyReview.Database.Persitance;

namespace WeeklyReview.Shared.Services
{
    internal partial class ActivityChangeService : IActivityChangeService
    {
        private readonly WeeklyReviewDbContext _db;
        private readonly ITimeService _timeService;

        public ActivityChangeService(WeeklyReviewDbContext dataService, ITimeService timeService)
        {
            _db = dataService;
            _timeService = timeService;
        }

        public ActivityChangeModel ChangeActivity(ActivityModel source, ActivityModel destination, Guid userGuid)
        {
            DeleteActivity(source);
            OverrideEntries(source, destination, userGuid);

            var change = AddChange(source, destination, userGuid);
            _db.SaveChanges();
            return change;
        }

        private void OverrideEntries(ActivityModel source, ActivityModel destination, Guid userGuid)
        {
            var entries = _db.Entry.Include(x => x.Activities).Where(x => x.Activities.Contains(source) && x.Deleted == false).ToList();
            foreach (var entry in entries)
            {
                entry.Deleted = true;
            }

            foreach (var entry in entries)
            {
                var newEntry = new EntryModel(entry.StartTime, entry.EndTime, entry.RecordedTime, new List<ActivityModel>(), false, userGuid);
                _db.Entry.Add(newEntry);

                var activities = entry.Activities;
                activities.Remove(source);
                activities.Add(destination);
                newEntry.Activities = activities;
            }
        }

        private void DeleteActivity(ActivityModel source)
        {
            source.Deleted = true;
        }

        private ActivityChangeModel AddChange(ActivityModel source, ActivityModel destination, Guid userGuid)
        {
            var change = new ActivityChangeModel(null, null, _timeService.Current, userGuid);
            _db.ActivityChange.Add(change);
            change.Source = source;
            change.Destination = destination;
            return change;
        }
    }
}
