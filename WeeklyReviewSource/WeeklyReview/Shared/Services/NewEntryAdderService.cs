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

        public Task AddEntry(DateTime date, List<ActivityModel> activities, Guid userGuid)
        {
            throw new NotImplementedException();
        }
    }
}
