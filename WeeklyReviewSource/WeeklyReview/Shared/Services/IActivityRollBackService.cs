using WeeklyReview.Database.Models;

namespace WeeklyReview.Shared.Services
{
    public interface IActivityRollBackService
    {
        void RollBackActivityChange(ActivityChangeModel activityChange);
    }
}
