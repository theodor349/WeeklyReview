using WeeklyReview.Shared.Models;

namespace WeeklyReview.Shared.Services
{
    public interface IEntryParserService
    {
        (List<Activity> usedActivities, List<Category> usedCategories, List<Activity> newActivities, List<Category> newCategories) ParseEntries(IEnumerable<string> entries, IEnumerable<Activity> activities, IEnumerable<Category> categories, Category defaultCategory);
    }
}