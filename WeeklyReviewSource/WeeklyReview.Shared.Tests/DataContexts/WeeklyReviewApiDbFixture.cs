using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;
using WeeklyReview.Database.Persitance;
using WeeklyReview.Server.Persitance;

namespace WeeklyReview.Shared.Tests.DataContexts
{
    public class WeeklyReviewApiDbFixture
    {
        private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=EFTestSample;Trusted_Connection=True";
        private static readonly object _lock = new();
        private static bool _databaseInitialized;

        private DateTime _dt;
        public DateTime Dt => _dt;

        public WeeklyReviewDbContext CreateContext()
            => new WeeklyReviewApiDbContext(
            new DbContextOptionsBuilder<WeeklyReviewApiDbContext>()
                .UseSqlServer(ConnectionString)
                .Options);

        public WeeklyReviewApiDbFixture()
        {
            lock (_lock)
            {
                if (!_databaseInitialized)
                {
                    var cats = GenerateCategories();
                    var acts = GenerateActivities(cats);
                    var ents = GenerateEntries(acts);
                    var accs = GenerateActivityChanges(acts);
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        using (var transaction = context.Database.BeginTransaction())
                        {
                            context.Category.AddRange(cats.ToList().ConvertAll(x => x.Value));
                            context.Activity.AddRange(acts.ToList().ConvertAll(x => x.Value));
                            context.Entry.AddRange(ents.ToList().ConvertAll(x => x.Value));
                            context.ActivityChange.Add(accs.ToList().ConvertAll(x => x.Value).First());
                            context.SaveChanges();
                            transaction.Commit();
                        }
                    }

                    _databaseInitialized = true;
                }
            }
        }

        public void EnableIdentityInsertAll(WeeklyReviewDbContext context, bool enable)
        {
            SetIdentityInsertAsync<CategoryModel>(context, enable);
            SetIdentityInsertAsync<ActivityModel>(context, enable);
            SetIdentityInsertAsync<EntryModel>(context, enable);
            SetIdentityInsertAsync<ActivityChangeModel>(context, enable);
        }

        // https://stackoverflow.com/questions/40896047/how-to-turn-on-identity-insert-in-net-core#:~:text=Improved%20solution%20based%20on%20NinjaCross%27%20answer
        public void SetIdentityInsertAsync<TEnt>(WeeklyReviewDbContext context, bool enable)
        {
            var entityType = context.Model.FindEntityType(typeof(TEnt));
            var value = enable ? "ON" : "OFF";
            //string query = $"SET IDENTITY_INSERT {entityType.GetSchema()}.{entityType.GetTableName()} {value}";
            string query = $"SET IDENTITY_INSERT {entityType.GetTableName()} {value}";
            context.Database.ExecuteSqlRaw(query);
        }

        private SortedDictionary<int, ActivityChangeModel> GenerateActivityChanges(SortedDictionary<int, ActivityModel> acts)
        {
            return new SortedDictionary<int, ActivityChangeModel>()
            {
                { 1, new ActivityChangeModel(0, acts[3], acts[5], _dt.AddHours(6)) },
            };
        }

        private SortedDictionary<int, EntryModel> GenerateEntries(SortedDictionary<int, ActivityModel> acts)
        {
            _dt = new DateTime(2023, 8, 1, 10, 0, 0);
            return new SortedDictionary<int, EntryModel>()
            {
                { 1, new EntryModel(0, _dt, _dt.AddHours(1), new List<ActivityModel>() { acts[1], acts[6] }, false) },
                { 2, new EntryModel(0, _dt.AddHours(1), _dt.AddHours(2), new List<ActivityModel>() { acts[2] }, false) },
                { 3, new EntryModel(0, _dt.AddHours(2), _dt.AddHours(3), new List<ActivityModel>() { acts[3], acts[4] }, true) },
                { 4, new EntryModel(0, _dt.AddHours(3), _dt.AddHours(4), new List<ActivityModel>() { acts[3] }, true) },
                { 5, new EntryModel(0, _dt.AddHours(4), _dt.AddHours(5), new List<ActivityModel>() { acts[5], acts[7] }, false) },
                { 6, new EntryModel(0, _dt.AddHours(2), _dt.AddHours(3), new List<ActivityModel>() { acts[5], acts[4] }, true) },
                { 7, new EntryModel(0, _dt.AddHours(3), _dt.AddHours(4), new List<ActivityModel>() { acts[5] }, true) },
                { 8, new EntryModel(0, _dt.AddHours(2), _dt.AddHours(3), new List<ActivityModel>() { acts[1], acts[4] }, false) },
                { 9, new EntryModel(0, _dt.AddHours(3), _dt.AddHours(4), new List<ActivityModel>() { acts[5], acts[2] }, false) },
            };
        }

        private SortedDictionary<int, ActivityModel> GenerateActivities(SortedDictionary<int, CategoryModel> cats)
        {
            return new SortedDictionary<int, ActivityModel>()
            {
                { 1, new ActivityModel(0, "Dinner", false, cats[1]) },
                { 2, new ActivityModel(0, "Lunch", false, cats[1]) },
                { 3, new ActivityModel(0, "Bike", true, cats[2]) },
                { 4, new ActivityModel(0, "Shopping", false, cats[3]) },
                { 5, new ActivityModel(0, "Swim", false, cats[2]) },
                { 6, new ActivityModel(0, "Youtube", false, cats[4]) },
                { 7, new ActivityModel(0, "TV", false, cats[4]) },
            };
        }

        private SortedDictionary<int, CategoryModel> GenerateCategories()
        {
            return new SortedDictionary<int, CategoryModel>()
            {
                { 1, new CategoryModel(0, "Food", 1, Color.Yellow) },
                { 2, new CategoryModel(0, "Sports", 2, Color.Green) },
                { 3, new CategoryModel(0, "Shopping", 2, Color.Blue) },
                { 4, new CategoryModel(0, "Television", 2, Color.Red) },
            };
        }
    }
}
