using database.Models;

namespace Shared.Services;

public interface IEntryParserService
{
    Task<List<ActivityModel>> ParseEntry(List<string> activities, Guid userGuid);
}