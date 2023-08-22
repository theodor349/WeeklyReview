using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;

namespace WeeklyReview.Database.Persitance
{
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
}
