using WeeklyReview.Shared.Models.DTOs;

namespace WeeklyReview.Client.Services
{
    public interface IWeeklyReviewServiceOld
    {
        List<ActivityDto> Activities { get; }
        List<CategoryDto> Categories { get; }
        List<EntryDto> Entries { get; }
        List<string> Socials { get; }

        Task AddEntry(DateTime date, List<string> activities);
    }
}