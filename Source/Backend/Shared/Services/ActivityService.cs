using database.Models;
using database.Persitance;
using Microsoft.EntityFrameworkCore;

namespace shared.Services;

internal class ActivityService : IActivityService
{
    private readonly WeeklyReviewDbContext _db;
    private readonly IActivityChangeService _activityChangeService;

    public ActivityService(WeeklyReviewDbContext db, IActivityChangeService activityChangeService)
    {
        _db = db;
        _activityChangeService = activityChangeService;
    }

    public async Task<IEnumerable<ActivityModel>> GetAll(Guid userGuid)
    {
        return await _db.Activity.Where(x => x.UserGuid == userGuid).OrderBy(x => x.NormalizedName).ToListAsync();
    }

    public async Task<ActivityModel?> Get(int key, Guid userGuid)
    {
        return await _db.Activity.SingleOrDefaultAsync(x => x.Id == key && x.UserGuid == userGuid);
    }

    public async Task<ActivityModel> Delete(int key, Guid userGuid)
    {
        var model = await _db.Activity.SingleOrDefaultAsync(x => x.Id == key && x.UserGuid == userGuid);
        if (model is null)
            throw new KeyNotFoundException($"Model not found with id {key}");

        var entriesReferencesActivity = _db.Entry.Include(x => x.Activities).Any(x => x.Activities.Contains(model) && x.Deleted == false);
        if (entriesReferencesActivity)
            throw new InvalidOperationException($"It is not possible to delete an activity which is still referenced by entries");

        model.Deleted = true;
        await _db.SaveChangesAsync();
        return model;
    }

    public async Task<ActivityChangeModel> Convert(int sKey, int dKey, Guid userGuid)
    {
        return await _activityChangeService.ChangeActivity(sKey, dKey, userGuid);
    }
}
