using WeeklyReview.Shared.Models.DTOs;

namespace WeeklyReview.Shared.Services
{
    public interface IEntryParserService
    {
        (List<ActivityDto> usedActivities, List<CategoryDto> usedCategories, List<ActivityDto> newActivities, List<CategoryDto> newCategories) ParseEntries(IEnumerable<string> entries, IEnumerable<ActivityDto> activities, IEnumerable<CategoryDto> categories, CategoryDto defaultCategory);
    }
}