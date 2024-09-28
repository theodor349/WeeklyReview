using database.Models;

namespace Shared.Services;

public interface IEntryAdderService
{
    Task<EntryModel?> AddEntry(DateTime date, List<ActivityModel> activities, Guid userGuid);
}