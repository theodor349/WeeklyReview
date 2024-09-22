using database.Models;
using database.Persitance;
using Microsoft.EntityFrameworkCore;

namespace shared.Services;

internal class EntryAdderService : IEntryAdderService
{
    private readonly WeeklyReviewDbContext _db;
    private readonly ITimeService _timeService;

    public EntryAdderService(WeeklyReviewDbContext db, ITimeService timeService)
    {
        _db = db;
        _timeService = timeService;
    }

    public async Task<EntryModel?> AddEntry(DateTime date, List<ActivityModel> activities, Guid userGuid)
    {
        date = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute / 15 * 15, 0, date.Kind);

        EntryModel? res = null;
        if (activities.Count == 0)
        {
            await DeleteEntryAt(date, userGuid);
            var endTime = await GetEndTime(date, userGuid);
            await UpdateBefore(date, endTime, userGuid);
        }
        else
        {
            await DeleteEntryAt(date, userGuid);
            var endTime = await GetEndTime(date, userGuid);
            await UpdateBefore(date, date, userGuid);
            res = AddNewEntry(date, activities, userGuid, endTime);
        }
        await _db.SaveChangesAsync();
        return res;
    }

    private EntryModel AddNewEntry(DateTime date, List<ActivityModel> activities, Guid userGuid, DateTime? endTime)
    {
        var entry = new EntryModel(date, endTime, _timeService.Current, new List<ActivityModel>(), false, userGuid);
        _db.Entry.Add(entry);
        entry.Activities = activities;
        return entry;
    }

    private async Task<EntryModel?> DeleteEntryAt(DateTime date, Guid userGuid)
    {
        var entry = await _db.Entry.FirstOrDefaultAsync(x => x.StartTime == date && x.UserGuid == userGuid && x.Deleted == false);
        if (entry is not null)
            entry.Deleted = true;
        return entry;
    }

    private async Task<DateTime?> GetEndTime(DateTime date, Guid userGuid)
    {
        var res = await _db.Entry
            .Where(x => x.StartTime > date && x.Deleted == false && x.UserGuid == userGuid)
            .MinAsync(x => (DateTime?)x.StartTime);
        if (res == DateTime.MinValue)
            return null;
        else
            return res;
    }

    private async Task UpdateBefore(DateTime date, DateTime? endTime, Guid userGuid)
    {
        var entries = await _db.Entry
            .Where(x => x.StartTime < date && x.Deleted == false && x.UserGuid == userGuid)
            .ToListAsync();
        var entry = entries.MaxBy(x => x.StartTime);

        if (entry is not null)
            entry.EndTime = endTime;
    }
}
