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
            var entry = new EntryModel(date, null, _timeService.Current, activities, false, userGuid);
            _db.Entry.Add(entry);
            _db.SaveChanges();
            return entry;
        }
    }
}
