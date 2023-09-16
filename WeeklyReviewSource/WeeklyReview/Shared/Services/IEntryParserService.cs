using WeeklyReview.Database.Models;

namespace WeeklyReview.Shared.Services
{
    public interface IEntryParserService
    {
        List<ActivityModel> ParseEntry(List<string> activities, Guid userGuid);
    }
}