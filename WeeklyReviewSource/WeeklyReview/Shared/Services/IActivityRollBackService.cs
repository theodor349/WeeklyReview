using WeeklyReview.Shared.Models;

namespace WeeklyReview.Shared.Services
{
    public interface IActivityRollBackService
    {
        Task RollBackActivityChange(ActivityChangeModel activityChange);
    }
}
