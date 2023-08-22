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
    internal class ActivityRollBackService : IActivityRollBackService
    {
        private readonly WeeklyReviewDbContext _db;
        private readonly ITimeService _timeService;

        public ActivityRollBackService(WeeklyReviewDbContext dataService, ITimeService timeService)
        {
            _db = dataService;
            _timeService = timeService;
        }

        public void RollBackActivityChange(ActivityChangeModel activityChange)
        {
            var oldActivity = _db.Activity.Single(x => x.Id == activityChange.Source.Id);
            oldActivity.Deleted = false;
            var oldEntries = _db.Entry
                .Where(x => x.Activities.Any(x => x.Id == activityChange.Source.Id)).ToList();
            foreach (var entry in oldEntries)
            {
                RollBackEntry(activityChange.Source, activityChange.Destination, entry);
            }

            _db.ActivityChange.Remove(activityChange);
            _db.SaveChanges();
        }

        private void RollBackEntry(ActivityModel originalAct, ActivityModel overrideAct, EntryModel oldEntry)
        {
            var newerEntries = _db.Entry
                .Include(x => x.Activities)
                .Where(x => x.StartTime == oldEntry.StartTime && x.RecordedTime > oldEntry.RecordedTime);

            foreach (var entry in newerEntries)
            {
                if (!entry.Activities.Contains(overrideAct))
                    return;
            }

            OverrideEntry(originalAct, overrideAct, newerEntries);
        }

        private void OverrideEntry(ActivityModel originalAct, ActivityModel overrideAct, IQueryable<EntryModel> newerEntries)
        {
            var newestEntry = newerEntries.Single(x => x.Deleted == false);
            newestEntry.Deleted = true;
            _db.Entry.Update(newestEntry);

            var activities = newestEntry.Activities.Where(x => x.Id != overrideAct.Id).ToList();
            activities.Add(originalAct);
            _db.Add(new EntryModel(newestEntry.StartTime, newestEntry.EndTime, _timeService.Current, activities, false, newestEntry.UserGuid));
        }
    }
}
