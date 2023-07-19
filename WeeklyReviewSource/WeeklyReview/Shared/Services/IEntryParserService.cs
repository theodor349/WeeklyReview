using WeeklyReview.Shared.Models;

namespace WeeklyReview.Shared.Services
{
    public interface IEntryParserService
    {
        (List<Activity> usedActivities, List<Category> usedCategories, List<Activity> newActivities, List<Category> newCategories) ParseEntries(List<string> entries, List<Activity> activities, List<Category> categories, Category defaultCategory);
    }
}