using database.Models;
using Shared.Models;
using System.Drawing;

namespace Shared.Services;

public interface IWeeklyReviewService
{
    public IActivityChangeService ActivityChange { get; }
    public IActivityService Activity { get; }
    public ICategoryService Category { get; }
    public IEntryService Entry { get; }
}

public class WeeklyReviewService : IWeeklyReviewService
{
    public IActivityChangeService ActivityChange { get; private set; }

    public IActivityService Activity { get; private set; }

    public ICategoryService Category { get; private set; }

    public IEntryService Entry { get; private set; }

    public WeeklyReviewService(IActivityChangeService activityChange, IActivityService activity, ICategoryService category, IEntryService entry)
    {
        ActivityChange = activityChange;
        Activity = activity;
        Category = category;
        Entry = entry;
    }
}

public interface IActivityService
{
    Task<ActivityChangeModel> Convert(int sKey, int dKey, Guid userGuid);
    Task<ActivityModel> Delete(int key, Guid userGuid);
    Task<ActivityModel?> Get(int key, Guid userGuid);
    Task<IEnumerable<ActivityModel>> GetAll(Guid userGuid);
}

public interface ICategoryService
{
    Task<CategoryModel> ChangeColor(int key, Color color, Guid userGuid);
    Task<CategoryModel> Delete(int key, Guid userGuid);
    Task<CategoryModel?> Get(int key, Guid userGuid);
    Task<IEnumerable<CategoryModel>> GetAll(Guid userGuid);
}

public interface IEntryService
{
    Task<EntryModel?> Create(EnterEntryModel model, Guid userGuid);
    Task<EntryModel?> Get(int key, Guid userGuid);
    Task<IEnumerable<EntryModel>> GetAll(Guid userGuid);
    Task<IEnumerable<EntryModel>> GetAllAroundDate(Guid userGuid, DateTime date, int daysAround);
}

public interface IActivityChangeService
{
    Task<ActivityChangeModel> ChangeActivity(int sourceKey, int destinationKey, Guid userGuid);
    Task RollBackActivityChange(int key, Guid UserGuid);
    Task<ActivityChangeModel> Delete(int key, Guid UserGuid);
    Task<ActivityChangeModel?> Get(int key, Guid UserGuid);
    Task<IEnumerable<ActivityChangeModel>> GetAll(Guid UserGuid);
}
