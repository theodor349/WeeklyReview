using Database.Models;
using Database.Persitance;
using Microsoft.EntityFrameworkCore;

namespace Shared.Services;

internal partial class ActivityChangeService : IActivityChangeService
{
    private readonly WeeklyReviewDbContext _db;
    private readonly ITimeService _timeService;

    public ActivityChangeService(WeeklyReviewDbContext dataService, ITimeService timeService)
    {
        _db = dataService;
        _timeService = timeService;
    }

    public async Task<ActivityChangeModel> ChangeActivity(int sourceKey, int destinationKey, Guid userGuid)
    {
        var source = await _db.Activity.SingleOrDefaultAsync(x => x.Id == sourceKey);
        var destination = await _db.Activity.SingleOrDefaultAsync(x => x.Id == destinationKey);
        if (source is null)
            throw new KeyNotFoundException($"Model not found with id {sourceKey}");
        if (destination is null)
            throw new KeyNotFoundException($"Model not found with id {destinationKey}");

        DeleteActivity(source);
        await OverrideEntries(source, destination, userGuid);

        var change = await AddChange(source, destination, userGuid);
        await _db.SaveChangesAsync();
        return change;
    }

    private async Task OverrideEntries(ActivityModel source, ActivityModel destination, Guid userGuid)
    {
        var entries = await _db.Entry.Include(x => x.Activities).Where(x => x.Activities.Contains(source) && x.Deleted == false).ToListAsync();
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

    private async Task<ActivityChangeModel> AddChange(ActivityModel source, ActivityModel destination, Guid userGuid)
    {
        var change = new ActivityChangeModel(null, null, _timeService.Current, userGuid);
        await _db.ActivityChange.AddAsync(change);
        change.Source = source;
        change.Destination = destination;
        return change;
    }

    public async Task RollBackActivityChange(int key, Guid userGuid)
    {
        var activityChange = await _db.ActivityChange
            .Include(x => x.Source)
            .Include(x => x.Destination)
            .SingleOrDefaultAsync(x => x.Id == key && x.UserGuid == userGuid);
        if (activityChange is null)
            throw new KeyNotFoundException($"Model not found with id {key}");


        var oldActivity = await _db.Activity.SingleAsync(x => x.Id == activityChange.Source.Id);
        oldActivity.Deleted = false;
        var oldEntries = await _db.Entry
            .Where(x => x.Activities.Any(x => x.Id == activityChange.Source.Id)).ToListAsync();
        foreach (var entry in oldEntries)
        {
            await RollBackEntry(activityChange.Source, activityChange.Destination, entry);
        }

        _db.ActivityChange.Remove(activityChange);
        _db.SaveChanges();
    }

    private async Task RollBackEntry(ActivityModel originalAct, ActivityModel overrideAct, EntryModel oldEntry)
    {
        var newerEntries = _db.Entry
            .Include(x => x.Activities)
            .Where(x => x.StartTime == oldEntry.StartTime && x.RecordedTime > oldEntry.RecordedTime);

        foreach (var entry in newerEntries)
        {
            if (!entry.Activities.Contains(overrideAct))
                return;
        }

        await OverrideEntry(originalAct, overrideAct, newerEntries);
    }

    private async Task OverrideEntry(ActivityModel originalAct, ActivityModel overrideAct, IQueryable<EntryModel> newerEntries)
    {
        var newestEntry = await newerEntries.SingleOrDefaultAsync(x => x.Deleted == false);
        if (newestEntry is null)
            return;

        newestEntry.Deleted = true;
        _db.Entry.Update(newestEntry);

        var activities = newestEntry.Activities.Where(x => x.Id != overrideAct.Id).ToList();
        activities.Add(originalAct);
        await _db.AddAsync(new EntryModel(newestEntry.StartTime, newestEntry.EndTime, _timeService.Current, activities, false, newestEntry.UserGuid));
    }

    // TODO: Test
    public async Task<ActivityChangeModel> Delete(int key, Guid userGuid)
    {
        var model = await _db.ActivityChange.SingleOrDefaultAsync(x => x.Id == key && x.UserGuid == userGuid);
        if (model is null)
            throw new KeyNotFoundException($"Model not found with id {key}");

        _db.ActivityChange.Remove(model);
        await _db.SaveChangesAsync();
        return model!;
    }

    public async Task<ActivityChangeModel?> Get(int key, Guid userGuid)
    {
        return await _db.ActivityChange.SingleOrDefaultAsync(x => x.Id == key && x.UserGuid == userGuid);
    }

    public async Task<IEnumerable<ActivityChangeModel>> GetAll(Guid userGuid)
    {
        return await _db.ActivityChange.Where(x => x.UserGuid == userGuid).ToListAsync();
    }
}
