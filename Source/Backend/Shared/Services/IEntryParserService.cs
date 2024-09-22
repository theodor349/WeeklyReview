using database.Models;

namespace shared.Services;

public interface IEntryParserService
{
    Task<List<ActivityModel>> ParseEntry(List<string> activities, Guid userGuid);
}