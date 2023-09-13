using WeeklyReview.Database.Models;

namespace WeeklyReview.Shared.Services
{
    public interface IActivityChangeService
    {
        ActivityChangeModel ChangeActivity(ActivityModel source, ActivityModel destination, Guid userGuid);
        void RollBackActivityChange(ActivityChangeModel activityChange);
    }
}
