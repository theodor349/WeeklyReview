using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;
using WeeklyReview.Database.Persitance;

namespace WeeklyReview.Shared.Services
{
    internal class NewEntryAdderService : INewEntryAdderService
    {
        private readonly WeeklyReviewDbContext _db;
        private readonly ITimeService _timeService;

        public NewEntryAdderService(WeeklyReviewDbContext db, ITimeService timeService)
        {
            _db = db;
            _timeService = timeService;
        }

        public EntryModel? AddEntry(DateTime date, List<ActivityModel> activities, Guid userGuid)
        {
            date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute / 15 * 15, 0, date.Kind);

            EntryModel? res = null;
            if(activities.Count == 0)
            {
                DeleteEntryAt(date, userGuid);
                var endTime = GetEndTime(date, userGuid);
                UpdateBefore(date, endTime, userGuid);
            }
            else
            {
                DeleteEntryAt(date, userGuid);
                var endTime = GetEndTime(date, userGuid);
                UpdateBefore(date, date, userGuid);
                res = AddNewEntry(date, activities, userGuid, endTime);
            }
            _db.SaveChanges();
            return res;
        }

        private EntryModel AddNewEntry(DateTime date, List<ActivityModel> activities, Guid userGuid, DateTime? endTime)
        {
            var entry = new EntryModel(date, endTime, _timeService.Current, new List<ActivityModel>(), false, userGuid);
            _db.Entry.Add(entry);
            _db.SaveChanges(); // This needs to happen before we add the activities
            entry.Activities = activities;
            _db.SaveChanges();
            return entry;
        }

        private EntryModel? DeleteEntryAt(DateTime date, Guid userGuid)
        {
            var entry = _db.Entry.FirstOrDefault(x => x.StartTime == date && x.UserGuid == userGuid && x.Deleted == false);
            if (entry is not null)
                entry.Deleted = true;
            return entry;
        }

        private DateTime? GetEndTime(DateTime date, Guid userGuid)
        {
            var res = _db.Entry
                .Where(x => x.StartTime > date && x.Deleted == false && x.UserGuid == userGuid)
                .Min(x => (DateTime?) x.StartTime);
            if(res == DateTime.MinValue)
                return null;
            else 
                return res;
        }

        private void UpdateBefore(DateTime date, DateTime? endTime, Guid userGuid)
        {
            var entry = _db.Entry
                .Where(x => x.StartTime < date && x.Deleted == false && x.UserGuid == userGuid)
                .AsEnumerable()
                .MaxBy(x => x.StartTime);

            if(entry is not null)
                entry.EndTime = endTime;
        }
    }
}
