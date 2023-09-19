using Microsoft.EntityFrameworkCore;
using WeeklyReview.Database.Models;

namespace WeeklyReview.Database.Persitance
{
    public interface IWeeklyReviewDbContext
    {
        DbSet<ActivityModel> Activity { get; set; }
        DbSet<ActivityChangeModel> ActivityChange { get; set; }
        DbSet<CategoryModel> Category { get; set; }
        DbSet<EntryModel> Entry { get; set; }

        int SaveChanges();
    }
}