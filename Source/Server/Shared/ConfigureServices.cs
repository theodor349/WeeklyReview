using Microsoft.Extensions.DependencyInjection;
using Shared.Services;

namespace Shared;

public static class ConfigureServices
{
    public static void AddSharedServices(this IServiceCollection services)
    {
        services.AddTransient<IEntryAdderService, EntryAdderService>();
        services.AddTransient<IEntryParserService, EntryParserService>();
        services.AddTransient<IEntryService, EntryService>();
        services.AddTransient<IActivityChangeService, ActivityChangeService>();
        services.AddTransient<IActivityService, ActivityService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<ITimeService, TimeService>();
        services.AddTransient<IWeeklyReviewService, WeeklyReviewService>();
    }
}
