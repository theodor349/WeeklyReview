using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

        public void RollBackActivityChange(int key, Guid userGuid)
        {
            var activityChange = _db.ActivityChange
                .Include(x => x.Source)
                .Include(x => x.Destination)
                .SingleOrDefault(x => x.Id == key && x.UserGuid == userGuid);
            if (activityChange is null)
                throw new KeyNotFoundException($"Model not found with id {key}");


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
            var newestEntry = newerEntries.SingleOrDefault(x => x.Deleted == false);
            if (newestEntry is null)
                return;

            newestEntry.Deleted = true;
            _db.Entry.Update(newestEntry);

            var activities = newestEntry.Activities.Where(x => x.Id != overrideAct.Id).ToList();
            activities.Add(originalAct);
            _db.Add(new EntryModel(newestEntry.StartTime, newestEntry.EndTime, _timeService.Current, activities, false, newestEntry.UserGuid));
        }

        public ActivityChangeModel Remove(int key, Guid userGuid)
        {
            var model = _db.ActivityChange.SingleOrDefault(x => x.Id == key && x.UserGuid == userGuid);
            if (model is null)
                throw new KeyNotFoundException($"Model not found with id {key}");

            _db.ActivityChange.Remove(model);
            _db.SaveChanges();
            return model!;
        }

        public ActivityChangeModel? Get(int key, Guid userGuid)
        {
            return _db.ActivityChange.SingleOrDefault(x => x.Id == key && x.UserGuid == userGuid);
        }

        public IEnumerable<ActivityChangeModel> GetAll(Guid userGuid)
        {
            return _db.ActivityChange.Where(x => x.UserGuid == userGuid);
        }
    }
}
