using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;
using WeeklyReview.Database.Persitance;

namespace WeeklyReview.Shared.Services
{
    internal class ActivityRollBackService : IActivityRollBackService
    {
        private readonly WeeklyReviewDbContext _db;

        public ActivityRollBackService(WeeklyReviewDbContext dataService)
        {
            _db = dataService;
        }

        public void RollBackActivityChange(ActivityChangeModel activityChange)
        {
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
            var newestEntry = _db.Entry
                .Include(x => x.Activities)
                .Where(x => x.StartTime == oldEntry.StartTime)
                .Single(x => x.Deleted == false);
            newestEntry.Deleted = true;
            _db.Entry.Update(newestEntry);

            var activities = newestEntry.Activities.Where(x => x.Id != overrideAct.Id).ToList();
            activities.Add(originalAct);
            _db.Add(new EntryModel(0, newestEntry.StartTime, newestEntry.EndTime, activities, false));
        }
    }
}
