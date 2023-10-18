using System.Drawing;
using WeeklyReview.Client.Http;
using WeeklyReview.Database.Models;
using WeeklyReview.Shared.Models;
using WeeklyReview.Shared.Services;

namespace WeeklyReview.Client.Services
{
    public class ApiWeeklyReviewService : IApiWeeklyReviewService
    {
        public IActivityChangeService ActivityChange { get; set; }
        public IActivityService Activity { get; set; }
        public ICategoryService Category { get; set; }
        public IEntryService Entry { get; set; } 

        public ApiWeeklyReviewService(IActivityChangeService activityChange, IActivityService activity, ICategoryService category, IEntryService entry)
        {
            ActivityChange = activityChange;
            Activity = activity;
            Category = category;
            Entry = entry;
        }
    }

    public class EntryService : IEntryService
    {
        private readonly WeeklyReviewApiClient _client;

        public EntryService(WeeklyReviewApiClient client)
        {
            _client = client;
        }

        public async Task<EntryModel?> Create(EnterEntryModel model, Guid userGuid)
        {
            var res = await _client.POSTAsync<EntryModel?, EnterEntryModel>($"/api/v1/Entry/Enter", model, CancellationToken.None);
            if (res.IsEmpty)
                return null;
            else
                return res.Value;
        }

        public async Task<EntryModel?> Get(int key, Guid userGuid)
        {
            var res = await _client.GETAsync<EntryModel>($"/api/v1/Entry/{key}", CancellationToken.None);
            if (res.IsEmpty)
                return null;
            else
                return res.Value;
        }

        public async Task<IEnumerable<EntryModel>> GetAll(Guid userGuid)
        {
            var res = await _client.GETCollectionAsync<EntryModel>($"/api/v1/Entry", CancellationToken.None);
            if (res.IsEmpty)
                return null;
            else
                return res.Value;
        }

        public async Task<IEnumerable<EntryModel>> GetAllAroundDate(Guid userGuid, DateTime date, int daysAround)
        {
            var res = await _client.GETCollectionAsync<EntryModel>($"/api/v1/Entry/Around", CancellationToken.None);
            if (res.IsEmpty)
                return null;
            else
                return res.Value;
        }
    }

    public class ActivityChangeService : IActivityChangeService
    {
        private readonly WeeklyReviewApiClient _client;

        public ActivityChangeService(WeeklyReviewApiClient client)
        {
            _client = client;
        }

        public async Task<ActivityChangeModel> ChangeActivity(int sourceKey, int destinationKey, Guid userGuid)
        {
            var res = await _client.POSTAsync<ActivityChangeModel, object>($"/api/v1/Activity/{sourceKey}/ChangeTo/{destinationKey}", null, CancellationToken.None);
            if (res.IsEmpty)
                return null;
            else
                return res.Value;
        }

        public async Task<ActivityChangeModel> Delete(int key, Guid userGuid)
        {
            var res = await _client.DELETEAsync<ActivityChangeModel, object>($"/api/v1/ActivityChange/{key}", CancellationToken.None);
            if (res.IsEmpty)
                return null;
            else
                return res.Value;
        }

        public async Task<ActivityChangeModel?> Get(int key, Guid userGuid)
        {
            var res = await _client.GETAsync<ActivityChangeModel>($"/api/v1/ActivityChange/{key}", CancellationToken.None);
            if (res.IsEmpty)
                return null;
            else
                return res.Value;
        }

        public async Task<IEnumerable<ActivityChangeModel>> GetAll(Guid userGuid)
        {
            var res = await _client.GETCollectionAsync<ActivityChangeModel>($"/api/v1/ActivityChange", CancellationToken.None);
            if (res.IsEmpty)
                return null;
            else
                return res.Value;
        }

        public async Task RollBackActivityChange(int key, Guid UserGuid)
        {
            await _client.POSTAsync<CategoryModel, object>($"/api/v1/ActivityChange/{key}/Rollback", null, CancellationToken.None);
        }
    }

    public class CategoryService : ICategoryService
    {
        private readonly WeeklyReviewApiClient _client;

        public CategoryService(WeeklyReviewApiClient client)
        {
            _client = client;
        }

        public async Task<CategoryModel> ChangeColor(int key, Color color, Guid userGuid)
        {
            var res = await _client.POSTAsync<CategoryModel, Color>($"/api/v1/Category/{key}/ChangeColor", color, CancellationToken.None);
            if (res.IsEmpty)
                return null;
            else
                return res.Value;
        }

        public async Task<CategoryModel> Delete(int key, Guid userGuid)
        {
            var res = await _client.DELETEAsync<CategoryModel, object>($"/api/v1/Category/{key}", CancellationToken.None);
            if (res.IsEmpty)
                return null;
            else
                return res.Value;
        }

        public async Task<CategoryModel?> Get(int key, Guid userGuid)
        {
            var res = await _client.GETAsync<CategoryModel>($"/api/v1/Category/{key}", CancellationToken.None);
            if (res.IsEmpty)
                return null;
            else
                return res.Value;
        }

        public async Task<IEnumerable<CategoryModel>> GetAll(Guid userGuid)
        {
            var res = await _client.GETCollectionAsync<CategoryModel>($"/api/v1/Category", CancellationToken.None);
            if (res.IsEmpty)
                return null;
            else
                return res.Value;
        }
    }

    public class ActivityService : IActivityService
    {
        private readonly WeeklyReviewApiClient _client;

        public ActivityService(WeeklyReviewApiClient client)
        {
            _client = client;
        }

        public async Task<ActivityChangeModel> Convert(int sKey, int dKey, Guid userGuid)
        {
            var res = await _client.POSTAsync<ActivityChangeModel, object>($"/api/v1/Activity/{sKey}/ChangeTo/{dKey}", null, CancellationToken.None);
            if (res.IsEmpty)
                return null;
            else
                return res.Value;
        }

        public async Task<ActivityModel> Delete(int key, Guid userGuid)
        {
            var res = await _client.DELETEAsync<ActivityModel, object>($"/api/v1/Activity/{key}", CancellationToken.None);
            if (res.IsEmpty)
                return null;
            else
                return res.Value;
        }

        public async Task<ActivityModel?> Get(int key, Guid userGuid)
        {
            var res = await _client.GETAsync<ActivityModel>($"/api/v1/Activity/{key}", CancellationToken.None);
            if (res.IsEmpty)
                return null;
            else
                return res.Value;
        }

        public async Task<IEnumerable<ActivityModel>> GetAll(Guid userGuid)
        {
            var res = await _client.GETCollectionAsync<ActivityModel>($"/api/v1/Activity", CancellationToken.None);
            if (res.IsEmpty)
                return null;
            else
                return res.Value;
        }
    }
}
