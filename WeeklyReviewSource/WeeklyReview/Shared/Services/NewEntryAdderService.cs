using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;
using WeeklyReview.Database.Persitance;

namespace WeeklyReview.Shared.Services
{
    public class NewEntryAdderService
    {
        private readonly WeeklyReviewDbContext _db;
        private readonly ITimeService _timeService;

        public NewEntryAdderService(WeeklyReviewDbContext db, ITimeService timeService)
        {
            _db = db;
            _timeService = timeService;
        }

        public EntryModel AddEntry(DateTime date, List<ActivityModel> activities, Guid userGuid)
        {
            UpdateBefore(date, userGuid);
            var endTime = GetEndTime(date, userGuid);

            var entry = new EntryModel(date, endTime, _timeService.Current, activities, false, userGuid);
            _db.Entry.Add(entry);
            _db.SaveChanges();
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

        private void UpdateBefore(DateTime date, Guid userGuid)
        {
            var entry = _db.Entry
                .Where(x => x.StartTime < date && x.Deleted == false && x.UserGuid == userGuid)
                .AsEnumerable()
                .MaxBy(x => x.StartTime);

            if(entry is not null)
                entry.EndTime = date;
        }
    }
}
