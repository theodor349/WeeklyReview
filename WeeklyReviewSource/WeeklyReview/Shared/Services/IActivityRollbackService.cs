using WeeklyReview.Database.Models;

namespace WeeklyReview.Shared.Services
{
    public interface IActivityRollbackService
    {
        void RollBackActivityChange(ActivityChangeModel activityChange);
    }
}