using WeeklyReview.Shared.Models;

namespace WeeklyReview.Client.Services
{
    public interface IWeeklyReviewService
    {
        List<Activity> Activities { get; }
        List<Category> Categories { get; }
        List<Entry> Entries { get; }
        List<string> Socials { get; }

        Task AddEntry(DateTime date, List<string> activities);
    }
}