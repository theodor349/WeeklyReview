using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;

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

    }

    public interface ICategoryService
    {

    }

    public interface IEntryService
    {

    }

    public interface IActivityChangeService
    {
        ActivityChangeModel ChangeActivity(ActivityModel source, ActivityModel destination, Guid userGuid);
        void RollBackActivityChange(int key, Guid UserGuid);
        ActivityChangeModel Remove(int key, Guid UserGuid);
        ActivityChangeModel? Get(int key, Guid UserGuid);
        IEnumerable<ActivityChangeModel> GetAll(Guid UserGuid);
    }
}
