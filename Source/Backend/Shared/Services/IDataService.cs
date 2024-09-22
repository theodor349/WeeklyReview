using shared.Models.DTOs;

namespace shared.Services;

public interface IDataService
{
    public Task AddActivities(IEnumerable<ActivityDto> activities);
    public Task AddCategories(IEnumerable<CategoryDto> categories);

    public Task<IEnumerable<EntryDto>> GetEntries();
    public Task<IEnumerable<string>> GetSocials();
    public Task<IEnumerable<ActivityDto>> GetActivities();
    public Task<IEnumerable<CategoryDto>> GetCategories();
    public Task<CategoryDto> GetDefaultCategory();

    public Task<EntryDto?> GetBeforeEntry(DateTime date);
    public Task<EntryDto?> GetAfterEntry(DateTime date);
    public Task<EntryDto?> GetEqualEntry(DateTime date);
    public Task<EntryDto?> GetBeforeOrEqualEntry(DateTime date);

    public Task AddEntry(EntryDto entry);
    public Task RemoveEntry(EntryDto entry);
}
