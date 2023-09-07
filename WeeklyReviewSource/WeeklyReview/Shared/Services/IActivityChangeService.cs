using WeeklyReview.Database.Models;

namespace WeeklyReview.Shared.Services
{
    public interface IActivityChangeService
    {
        void RollBackActivityChange(ActivityChangeModel activityChange);
        ActivityChangeModel ChangeActivity(ActivityModel source, ActivityModel destination, Guid userGuid);
    }
}
