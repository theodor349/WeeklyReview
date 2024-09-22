using database.Models;

namespace shared.Services;

public interface IEntryAdderService
{
    Task<EntryModel?> AddEntry(DateTime date, List<ActivityModel> activities, Guid userGuid);
}