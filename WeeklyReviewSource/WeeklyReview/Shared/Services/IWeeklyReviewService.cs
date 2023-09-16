using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;
using WeeklyReview.Shared.Models;

namespace WeeklyReview.Shared.Services
{
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
        ActivityChangeModel Convert(int sKey, int dKey, Guid userGuid);
        ActivityModel Delete(int key, Guid userGuid);
        ActivityModel? Get(int key, Guid userGuid);
        IEnumerable<ActivityModel> GetAll(Guid userGuid);
    }

    public interface ICategoryService
    {
        CategoryModel ChangeColor(int key, Color color, Guid userGuid);
        CategoryModel Delete(int key, Guid userGuid);
        CategoryModel? Get(int key, Guid userGuid);
        IEnumerable<CategoryModel> GetAll(Guid userGuid);
    }

    public interface IEntryService
    {
        EntryModel? Create(EnterEntryModel model, Guid userGuid);
        EntryModel? Get(int key, Guid userGuid);
        IEnumerable<EntryModel> GetAll(Guid userGuid);
    }

    public interface IActivityChangeService
    {
        ActivityChangeModel ChangeActivity(int sourceKey, int destinationKey, Guid userGuid);
        void RollBackActivityChange(int key, Guid UserGuid);
        ActivityChangeModel Delete(int key, Guid UserGuid);
        ActivityChangeModel? Get(int key, Guid UserGuid);
        IEnumerable<ActivityChangeModel> GetAll(Guid UserGuid);
    }
}
