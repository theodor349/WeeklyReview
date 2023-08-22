using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Persitance;

namespace WeeklyReview.Shared.Services
{
    public class NewEntryAdderService : IEntryAdderService
    {
        private readonly WeeklyReviewDbContext _db;
        private readonly ITimeService _timeService;
        private readonly IEntryParserService _entryParserService;

        public NewEntryAdderService(WeeklyReviewDbContext db, ITimeService timeService, IEntryParserService entryParserService)
        {
            _db = db;
            _timeService = timeService;
            _entryParserService = entryParserService;
        }

        public Task AddEntry(DateTime date, List<string> activities)
        {
            throw new NotImplementedException();
        }
    }
}
