using WeeklyReview.Database.Models;

namespace WeeklyReview.Shared.Services
{
    public interface INewEntryParserService
    {
        List<ActivityModel> ParseEntry(List<string> activities, Guid userGuid);
    }
}