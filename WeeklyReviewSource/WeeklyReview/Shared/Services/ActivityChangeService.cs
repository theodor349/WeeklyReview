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
            return change;
        }

        private void OverrideEntries(ActivityModel source, ActivityModel destination, Guid userGuid)
        {
            //var entries = _db.Entry.Include(x => x.Activities).Where(x => x.Activities.Contains(source) && x.Deleted == false).ToList();
            var entries = _db.Entry.Include(x => x.Activities).Where(x => x.Activities.Contains(source)).ToList();
            foreach (var entry in entries)
            {
                entry.Deleted = true;
            }
            _db.SaveChanges();

            foreach (var entry in entries)
            {
                var newEntry = new EntryModel(entry.StartTime, entry.EndTime, entry.RecordedTime, new List<ActivityModel>(), false, userGuid);
                _db.Entry.Add(newEntry);

                var changes2 = _db.ChangeTracker.DebugView.LongView;
                var activities = entry.Activities;
                activities.Remove(source);
                activities.Add(destination);
                newEntry.Activities = activities;

                _db.SaveChanges();
            }
        }

        private void DeleteActivity(ActivityModel source)
        {
            var model = _db.Activity.Single(x => x.Id == source.Id);
            model.Deleted = true;
            _db.SaveChanges();
        }

        private ActivityChangeModel AddChange(ActivityModel source, ActivityModel destination, Guid userGuid)
        {
            var change = new ActivityChangeModel(source, destination, _timeService.Current, userGuid);
            _db.ActivityChange.Add(change);
            _db.SaveChanges();
            return change;
        }
    }
}
