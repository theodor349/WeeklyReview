using Database.Persitance;
using Microsoft.EntityFrameworkCore;

namespace Functions.Persitance;

public class WeeklyReviewApiDbContext : WeeklyReviewDbContext
{
    public WeeklyReviewApiDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(WeeklyReviewApiDbContext).Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }
}
