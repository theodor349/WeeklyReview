using database.Models;
using Microsoft.EntityFrameworkCore;

namespace database.Persitance;

public class WeeklyReviewDbContext : DbContext
{
    public WeeklyReviewDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<CategoryModel> Category { get; set; } = null;
    public DbSet<ActivityModel> Activity { get; set; } = null;
    public DbSet<ActivityChangeModel> ActivityChange { get; set; } = null;
    public DbSet<EntryModel> Entry { get; set; } = null;
}
