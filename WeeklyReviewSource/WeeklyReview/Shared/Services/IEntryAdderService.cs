using WeeklyReview.Database.Models;

namespace WeeklyReview.Shared.Services
{
    public interface IEntryAdderService
    {
        Task<EntryModel?> AddEntry(DateTime date, List<ActivityModel> activities, Guid userGuid);
    }
}