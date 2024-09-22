using database.Models;
using database.Persitance;
using Functions.Persitance;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace Shared.Tests.DataContexts;

public class WeeklyReviewApiDbFixtureForEntryAdderService
{
    private const string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=WrTestEntryAdder;Trusted_Connection=True";
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

    private DateTime _dt;
    public DateTime Dt => _dt;
    public DateTime MaxTime => _dt.AddHours(6);

    public WeeklyReviewDbContext CreateContext()
        => new WeeklyReviewApiDbContext(
        new DbContextOptionsBuilder<WeeklyReviewApiDbContext>()
            .UseSqlServer(ConnectionString)
            .Options);

    public WeeklyReviewApiDbFixtureForEntryAdderService()
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
                        AddCaseMovies(context);
                        context.SaveChanges();
                        AddCaseFoods(context);
                        context.SaveChanges();
                        AddCaseSports(context);
                        context.SaveChanges();
                        AddCaseTrip(context);
                        context.SaveChanges();
                        transaction.Commit();
                    }
                }

                _databaseInitialized = true;
            }
        }
    }

    private void AddCaseMovies(WeeklyReviewDbContext context)
    {
        var user = users[1];
        var defaultCategory = new CategoryModel("", 0, Color.White, user);

        var aSeries = new ActivityModel("Series", false, defaultCategory, user);
        var aMovie = new ActivityModel("Movie", false, defaultCategory, user);

        context.Category.AddRange(defaultCategory);
        context.Activity.AddRange(aMovie, aSeries);
    }

    private void AddCaseFoods(WeeklyReviewDbContext context)
    {
        var user = users[2];
        var defaultCategory = new CategoryModel("", 0, Color.White, user);
        var startTime = _dt;
        var endTime = startTime.AddHours(4);

        var aBreakfast = new ActivityModel("Breakfast", false, defaultCategory, user);
        var aLunch = new ActivityModel("Lunch", false, defaultCategory, user);
        var aDinner = new ActivityModel("Dinner", false, defaultCategory, user);
        var e1 = new EntryModel(startTime, endTime, startTime, aLunch, true, user);
        var e2 = new EntryModel(startTime, endTime, startTime, aBreakfast, false, user);
        var e3 = new EntryModel(startTime.AddHours(4), null, startTime, aLunch, false, user);

        context.Category.AddRange(defaultCategory);
        context.Activity.AddRange(aBreakfast, aLunch, aDinner);
        context.Entry.AddRange(e1, e2, e3);
    }

    private void AddCaseSports(WeeklyReviewDbContext context)
    {
        var user = users[3];
        var defaultCategory = new CategoryModel("", 0, Color.White, user);
        var startTime = _dt;
        var endTime = startTime.AddHours(4);

        var aRun = new ActivityModel("Run", false, defaultCategory, user);
        var aBike = new ActivityModel("Bike", false, defaultCategory, user);
        var aSwim = new ActivityModel("Swim", false, defaultCategory, user);
        var e1 = new EntryModel(startTime, endTime, startTime, aRun, false, user);
        var e2 = new EntryModel(endTime, endTime.AddHours(2), startTime, aBike, false, user);

        context.Category.AddRange(defaultCategory);
        context.Activity.AddRange(aRun, aBike, aSwim);
        context.Entry.AddRange(e1, e2);
    }

    private void AddCaseTrip(WeeklyReviewDbContext context)
    {
        var user = users[4];
        var defaultCategory = new CategoryModel("", 0, Color.White, user);
        var startTime = _dt;
        var endTime = startTime.AddHours(2);

        var aSpain = new ActivityModel("Spian", false, defaultCategory, user);
        var aFrance = new ActivityModel("France", false, defaultCategory, user);
        var aItaly = new ActivityModel("Italy", false, defaultCategory, user);
        var e1 = new EntryModel(startTime.AddHours(0), endTime.AddHours(0), startTime, aSpain, false, user);
        var e2 = new EntryModel(startTime.AddHours(2), endTime.AddHours(2), startTime, aFrance, false, user);
        var e3 = new EntryModel(startTime.AddHours(4), endTime.AddHours(4), startTime, aFrance, false, user);

        context.Category.AddRange(defaultCategory);
        context.Activity.AddRange(aSpain, aFrance, aItaly);
        context.Entry.AddRange(e1, e2, e3);
    }

}
