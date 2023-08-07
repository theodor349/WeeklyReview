using Microsoft.EntityFrameworkCore;
using WeeklyReview.Database.Persitance;

namespace WeeklyReview.Server.Persitance
{
    public class WeeklyReviewApiDbContext : WeeklyReviewDbContext
    {
        public WeeklyReviewApiDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(WeeklyReviewApiDbContext).Assembly);
        }
    }
}
