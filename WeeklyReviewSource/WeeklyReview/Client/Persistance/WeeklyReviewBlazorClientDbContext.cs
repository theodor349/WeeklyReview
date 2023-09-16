using Microsoft.EntityFrameworkCore;
using WeeklyReview.Database.Persitance;

namespace WeeklyReview.Client.Persistance
{
    public class WeeklyReviewBlazorClientDbContext : WeeklyReviewDbContext
    {
        public WeeklyReviewBlazorClientDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WeeklyReviewBlazorClientDbContext).Assembly);
        }
    }
}
