using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;
using WeeklyReview.Database.Persitance;
using WeeklyReview.Shared.Models;

namespace WeeklyReview.Shared.Services
{
    internal class EntryService : IEntryService
    {
        private readonly WeeklyReviewDbContext _db;
        private readonly IEntryAdderService _entryAdderService;
        private readonly IEntryParserService _entryParserService;

        public EntryService(WeeklyReviewDbContext db, IEntryAdderService entryAdderService, IEntryParserService entryParserService)
        {
            _db = db;
            _entryAdderService = entryAdderService;
            _entryParserService = entryParserService;
        }

        public IEnumerable<EntryModel> GetAll(Guid userGuid)
        {
            return _db.Entry.Include(x => x.Activities).Where(x => x.UserGuid == userGuid);
        }

        public EntryModel? Get(int key, Guid userGuid)
        {
            return _db.Entry.Include(x => x.Activities).SingleOrDefault(x => x.Id == key && x.UserGuid == userGuid);
        }

        public EntryModel? Create(EnterEntryModel model, Guid userGuid)
        {
            var activities = _entryParserService.ParseEntry(model.Entries, userGuid);
            var entry = _entryAdderService.AddEntry(model.Date, activities, userGuid);
            return entry;
        }
    }
}
