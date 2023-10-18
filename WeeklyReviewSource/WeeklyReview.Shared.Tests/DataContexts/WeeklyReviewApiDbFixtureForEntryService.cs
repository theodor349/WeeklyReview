using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WeeklyReview.Database.Models;
using WeeklyReview.Database.Persitance;
using WeeklyReview.Server.Persitance;

namespace WeeklyReview.Shared.Tests.DataContexts
{
    public class WeeklyReviewApiDbFixtureForEntryService
    {
        private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=WrTestEntry;Trusted_Connection=True";
        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        private List<Guid> users = new()
        {
            Guid.NewGuid(), // 0 (Not Used)
            Guid.NewGuid(), // 1
            Guid.NewGuid(), // 2
            Guid.NewGuid(), // 3
            Guid.NewGuid(), // 4
        };
        public List<Guid> Users => users;

        private DateTime _dt = new DateTime(2023, 1, 1);
        public DateTime Dt => _dt;
        public DateTime MaxTime => _dt.AddHours(6);

        public WeeklyReviewDbContext CreateContext()
            => new WeeklyReviewApiDbContext(
            new DbContextOptionsBuilder<WeeklyReviewApiDbContext>()
                .UseSqlServer(ConnectionString)
                .Options);

        public WeeklyReviewApiDbFixtureForEntryService()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        using (var transaction = context.Database.BeginTransaction())
                        {
                            AddPerson1(context);
                            context.SaveChanges();
                            AddPerson2Deleted(context);
                            context.SaveChanges();
                            AddPerson2(context);
                            context.SaveChanges();
                            transaction.Commit();
                        }
                    }

                    _databaseInitialized = true;
                }
            }
        }

        private void AddPerson1(WeeklyReviewDbContext context)
        {
            var user = users[1];
            var date = _dt;

            var defaultCategory = new CategoryModel("", 0, Color.White, user);

            var aSeries = new ActivityModel("Series", false, defaultCategory, user);

            var entries = new List<EntryModel>();
            int entryLength = 1;
            for (int i = 0; i < 20; i++)
            {
                var e = new EntryModel(date.AddDays(i * entryLength), date.AddDays((i + 1) * entryLength), _dt, aSeries, false, user);
                entries.Add(e);
            }

            context.Category.AddRange(defaultCategory);
            context.Activity.AddRange(aSeries);
            context.Entry.AddRange(entries);
        }

        private void AddPerson2Deleted(WeeklyReviewDbContext context)
        {
            var user = users[2];
            var date = _dt;

            var defaultCategory = new CategoryModel("", 0, Color.White, user);

            var aSeries = new ActivityModel("Series", false, defaultCategory, user);

            var entries = new List<EntryModel>();
            int entryLength = 1;
            for (int i = 0; i < 20; i++)
            {
                var e = new EntryModel(date.AddDays(i * entryLength), date.AddDays((i + 1) * entryLength), _dt, aSeries, true, user);
                entries.Add(e);
            }

            context.Category.AddRange(defaultCategory);
            context.Activity.AddRange(aSeries);
            context.Entry.AddRange(entries);
        }

        private void AddPerson2(WeeklyReviewDbContext context)
        {
            var user = users[2];
            var date = _dt;

            var defaultCategory = new CategoryModel("", 0, Color.White, user);

            var aSeries = new ActivityModel("Series", false, defaultCategory, user);

            var entries = new List<EntryModel>();
            int entryLength = 1;
            for (int i = 0; i < 20; i++)
            {
                var e = new EntryModel(date.AddDays(i * entryLength), date.AddDays((i + 1) * entryLength), _dt, aSeries, false, user);
                entries.Add(e);
            }

            context.Category.AddRange(defaultCategory);
            context.Activity.AddRange(aSeries);
            context.Entry.AddRange(entries);
        }
    }
}
