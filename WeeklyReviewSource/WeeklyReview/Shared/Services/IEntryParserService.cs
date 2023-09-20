using WeeklyReview.Database.Models;

namespace WeeklyReview.Shared.Services
{
    public interface IEntryParserService
    {
        Task<List<ActivityModel>> ParseEntry(List<string> activities, Guid userGuid);
    }
}