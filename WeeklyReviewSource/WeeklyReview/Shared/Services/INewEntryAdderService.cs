using WeeklyReview.Database.Models;

namespace WeeklyReview.Shared.Services
{
    public interface INewEntryAdderService
    {
        EntryModel? AddEntry(DateTime date, List<ActivityModel> activities, Guid userGuid);
    }
}