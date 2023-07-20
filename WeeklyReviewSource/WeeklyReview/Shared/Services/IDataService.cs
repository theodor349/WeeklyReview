using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Shared.Models;

namespace WeeklyReview.Shared.Services
{
    public interface IDataService
    {
        public Task AddActivities(IEnumerable<Activity> activities);
        public Task AddCategories(IEnumerable<Category> categories);

        public Task<IEnumerable<Entry>> GetEntries();
        public Task<IEnumerable<string>> GetSocials();
        public Task<IEnumerable<Activity>> GetActivities();
        public Task<IEnumerable<Category>> GetCategories();
        public Task<Category> GetDefaultCategory();

        public Task<Entry?> GetBeforeEntry(DateTime date);
        public Task<Entry?> GetAfterEntry(DateTime date);
        public Task<Entry?> GetEqualEntry(DateTime date);
        public Task<Entry?> GetBeforeOrEqualEntry(DateTime date);

        public Task AddEntry(Entry entry);
        public Task RemoveEntry(Entry entry);
    }
}
