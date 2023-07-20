namespace WeeklyReview.Shared.Services
{
    public interface IEntryAdderService
    {
        Task AddEntry(DateTime date, List<string> activities);
    }
}