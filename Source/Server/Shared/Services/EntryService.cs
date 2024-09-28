using Database.Models;
using Database.Persitance;
using Microsoft.EntityFrameworkCore;
using Shared.Models;
using Shared.Services;
using System.Data;

namespace Shared.Services;

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

    public async Task<IEnumerable<EntryModel>> GetAll(Guid userGuid)
    {
        var res = await _db.Entry.Include(x => x.Activities).ThenInclude(x => x.Category).Where(x => x.UserGuid == userGuid && x.Deleted == false).ToListAsync();
        return res;
    }

    public async Task<EntryModel?> Get(int key, Guid userGuid)
    {
        return await _db.Entry.Include(x => x.Activities).SingleOrDefaultAsync(x => x.Id == key && x.UserGuid == userGuid);
    }

    public async Task<EntryModel?> Create(EnterEntryModel model, Guid userGuid)
    {
        var activities = await _entryParserService.ParseEntry(model.Entries, userGuid);
        var entry = await _entryAdderService.AddEntry(model.Date, activities, userGuid);
        return entry;
    }

    public async Task<IEnumerable<EntryModel>> GetAllAroundDate(Guid userGuid, DateTime date, int daysAround)
    {
        var startDate = date.AddDays(-daysAround);
        var endDate = date.AddDays(daysAround);
        var res = await _db.Entry.Include(x => x.Activities).ThenInclude(x => x.Category).Where(x => x.UserGuid == userGuid && x.Deleted == false && startDate <= x.StartTime && x.StartTime <= endDate).ToListAsync();
        return res;
    }
}
