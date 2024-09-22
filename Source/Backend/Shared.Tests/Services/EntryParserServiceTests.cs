using Microsoft.EntityFrameworkCore;
using shared.Services;
using Shared.Tests.DataContexts;
using System.Drawing;

namespace Shared.Tests.Services;

public class EntryParserServiceTests : IClassFixture<WeeklyReviewApiDbFixtureForEntryParserService>
{
    public WeeklyReviewApiDbFixtureForEntryParserService DbFixture { get; }

    public EntryParserServiceTests(WeeklyReviewApiDbFixtureForEntryParserService dbFixture)
    {
        DbFixture = dbFixture;
    }

    [Fact]
    public async Task ParseEntry_Swim_Exists_CaseUser1()
    {
        int aSwim = 1;
        var user = DbFixture.Users[1];

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();
        var input = new List<string>()
        {
            "Swim"
        };

        // Act
        var sut = new EntryParserService(context);
        var res = await sut.ParseEntry(input, user);
        context.ChangeTracker.Clear();

        // Assert
        Assert.Single(res);
        Assert.Contains(res, x => x.Id == aSwim);
    }

    [Fact]
    public async Task ParseEntry_SwimWithSpace_Exists_CaseUser1()
    {
        int aSwim = 1;
        var user = DbFixture.Users[1];

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();
        var input = new List<string>()
        {
            " \t  Swim   \t"
        };

        // Act
        var sut = new EntryParserService(context);
        var res = await sut.ParseEntry(input, user);
        context.ChangeTracker.Clear();

        // Assert
        Assert.Single(res);
        Assert.Contains(res, x => x.Id == aSwim);
    }

    [Fact]
    public async Task ParseEntry_SportsBike_Exists_CaseUser1()
    {
        int aBike = 2;
        var user = DbFixture.Users[1];

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();
        var input = new List<string>()
        {
            "Sports: Bike"
        };

        // Act
        var sut = new EntryParserService(context);
        var res = await sut.ParseEntry(input, user);
        context.ChangeTracker.Clear();

        // Assert
        Assert.Single(res);
        Assert.Contains(res, x => x.Id == aBike);
    }

    [Fact]
    public async Task ParseEntry_SportsBikeAndDance_Exists_CaseUser1()
    {
        int aBike = 3;
        int aDance = 2;
        var user = DbFixture.Users[1];

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();
        var input = new List<string>()
        {
            "Sports: Bike",
            "Dance"
        };

        // Act
        var sut = new EntryParserService(context);
        var res = await sut.ParseEntry(input, user);
        context.ChangeTracker.Clear();

        // Assert
        Assert.Equal(2, res.Count());
        Assert.Contains(res, x => x.Id == aBike);
        Assert.Contains(res, x => x.Id == aDance);
    }

    [Fact]
    public async Task ParseEntry_SportsBikeAndExerciseRun_Exists_CaseUser1()
    {
        int aBike = 2;
        int aRun = 4;
        var user = DbFixture.Users[1];

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();
        var input = new List<string>()
        {
            "Sports: Bike",
            "Exercise: Run"
        };

        // Act
        var sut = new EntryParserService(context);
        var res = await sut.ParseEntry(input, user);
        context.ChangeTracker.Clear();

        // Assert
        Assert.Equal(2, res.Count());
        Assert.Contains(res, x => x.Id == aBike);
        Assert.Contains(res, x => x.Id == aRun);
    }

    [Fact]
    public async Task ParseEntry_BlankAndSportsBike_Exists_CaseUser1()
    {
        int aBike = 2;
        var user = DbFixture.Users[1];

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();
        var input = new List<string>()
        {
            "",
            "Sports: Bike",
        };

        // Act
        var sut = new EntryParserService(context);
        var res = await sut.ParseEntry(input, user);
        context.ChangeTracker.Clear();

        // Assert
        Assert.Single(res);
        Assert.Contains(res, x => x.Id == aBike);
    }

    [Fact]
    public async Task ParseEntry_DifficultCompetionsParticipatinginRaceWithSpaces_Exists_CaseUser1()
    {
        var user = DbFixture.Users[1];
        int aRace = 5;

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();
        var input = new List<string>()
        {
            "\t  Difficult  \t  Competions: \t Participating  \t in \t  Race \t"
        };
        // Act
        var sut = new EntryParserService(context);
        var res = await sut.ParseEntry(input, user);
        context.ChangeTracker.Clear();

        // Assert
        Assert.Single(res);
        Assert.Contains(res, x => x.Id == aRace);
    }

    [Theory]
    [InlineData(" ")]
    [InlineData(" \t ", " ")]
    public async Task ParseEntry_Blank_Exists_CaseUser1(params string[] input)
    {
        var user = DbFixture.Users[1];

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();

        // Act
        var sut = new EntryParserService(context);
        var res = await sut.ParseEntry(input.ToList(), user);
        context.ChangeTracker.Clear();

        // Assert
        Assert.Empty(res);
    }

    [Fact]
    public async Task ParseEntry_NoEntries_Exists_CaseUser1()
    {
        var user = DbFixture.Users[1];

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();

        // Act
        var sut = new EntryParserService(context);
        var res = await sut.ParseEntry(new List<string>(), user);
        context.ChangeTracker.Clear();

        // Assert
        Assert.Empty(res);
    }

    [Fact]
    public async Task ParseEntry_MultipleColons_Error_CaseUser1()
    {
        var user = DbFixture.Users[1];

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();
        var input = new List<string>()
        {
            "Sports: Bike: Fast"
        };
        // Act
        var sut = new EntryParserService(context);

        // Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await sut.ParseEntry(input, user));
        context.ChangeTracker.Clear();
        Assert.Equal("An entry cannot contain multiple ':'", exception.Message);
    }

    [Fact]
    public async Task ParseEntry_OnlyCategory_Error_CaseUser1()
    {
        var user = DbFixture.Users[1];

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();
        var input = new List<string>()
        {
            "Sports: "
        };
        // Act
        var sut = new EntryParserService(context);

        // Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await sut.ParseEntry(input, user));
        context.ChangeTracker.Clear();
        Assert.Equal("An entry must contain an Activity, if a category is supplied", exception.Message);
    }

    [Fact]
    public async Task ParseEntry_DoubleSwimEntry_OnlyOneSwim_CaseUser1()
    {
        var user = DbFixture.Users[1];
        int aSwim = 1;

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();

        // Act
        var sut = new EntryParserService(context);
        var res = await sut.ParseEntry(new List<string>()
        {
            "Swim",
            "Swim"
        }, user);
        context.ChangeTracker.Clear();

        // Assert
        Assert.Single(res);
        Assert.Contains(res, x => x.Id == aSwim);
    }

    [Fact]
    public async Task ParseEntry_ActivityExists_DoNothing_CaseUser2()
    {
        var user = DbFixture.Users[2];

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();
        var expectedActivityCount = context.Activity.Count();
        var expectedCategoryCount = context.Category.Count();

        // Act
        var sut = new EntryParserService(context);
        var res = await sut.ParseEntry(new List<string>()
        {
            "Sports: Bike"
        }, user);
        context.ChangeTracker.Clear();

        // Assert
        Assert.Equal(expectedActivityCount, context.Activity.Count());
        Assert.Equal(expectedCategoryCount, context.Category.Count());
    }

    [Fact]
    public async Task ParseEntry_SportsExist_RunDoesNotExist_AddRun_CaseUser2()
    {
        var user = DbFixture.Users[2];
        int cSports = 6;

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();
        var expectedActivityCount = context.Activity.Count() + 1;
        var expectedCategoryCount = context.Category.Count();

        // Act
        var sut = new EntryParserService(context);
        var res = await sut.ParseEntry(new List<string>()
        {
            "Sports: Run"
        }, user);
        context.ChangeTracker.Clear();

        // Assert
        var newActivity = context.Activity.Include(x => x.Category).Single(x => x.NormalizedName == "Sports: Run" && x.UserGuid == user);
        Assert.Equal("Sports: Run", newActivity.Name);
        Assert.Equal(cSports, newActivity.Category.Id);
        Assert.Equal(expectedActivityCount, context.Activity.Count());
        Assert.Equal(expectedCategoryCount, context.Category.Count());
    }

    [Fact]
    public async Task ParseEntry_NoDefaultCat_SwimDoesNotExist_AddSwim_CaseUser2()
    {
        var user = DbFixture.Users[2];

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();
        var expectedActivityCount = context.Activity.Count() + 1;

        // Act
        var sut = new EntryParserService(context);
        var res = await sut.ParseEntry(new List<string>()
        {
            "Swim"
        }, user);
        context.ChangeTracker.Clear();

        // Assert
        var newActivity = context.Activity.Include(x => x.Category).Single(x => x.NormalizedName == "Swim" && x.UserGuid == user);
        Assert.Equal("Swim", newActivity.Name);
        Assert.Equal(expectedActivityCount, context.Activity.Count());
    }

    [Fact]
    public async Task ParseEntry_NoDefaultCat_SwimDoesExist_AssignDefaultCategory_CaseUser2()
    {
        var user = DbFixture.Users[2];

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();
        var expectedCategoryCount = context.Category.Count();

        // Act
        var sut = new EntryParserService(context);
        var res = await sut.ParseEntry(new List<string>()
        {
            "Swim"
        }, user);
        context.ChangeTracker.Clear();

        // Assert
        var newActivity = context.Activity.Include(x => x.Category).Single(x => x.NormalizedName == "Swim" && x.UserGuid == user);
        var newCat = newActivity.Category;
        Assert.Equal("", newCat.Name);
        Assert.Equal(0, newCat.Priority);
        Assert.Equal(Color.White.R, newCat.Color.R);
        Assert.Equal(Color.White.G, newCat.Color.G);
        Assert.Equal(Color.White.B, newCat.Color.B);
        Assert.Equal(user, newCat.UserGuid);
        Assert.False(newCat.Deleted);
        Assert.Equal(expectedCategoryCount, context.Category.Count());
    }

    [Fact]
    public async Task ParseEntry_ExerciseDoesNotExist_BikeDoesNotExist_AddExerciseAndBike_CaseUser2()
    {
        var user = DbFixture.Users[2];
        int cExercise = 7;

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();
        var expectedActivityCount = context.Activity.Count() + 1;
        var expectedCategoryCount = context.Category.Count() + 1;

        // Act
        var sut = new EntryParserService(context);
        var res = await sut.ParseEntry(new List<string>()
        {
            "Exercise: Bike"
        }, user);
        context.ChangeTracker.Clear();

        // Assert
        var newActivity = context.Activity.Include(x => x.Category).Single(x => x.NormalizedName == "Exercise: Bike" && x.UserGuid == user);
        Assert.Equal("Exercise: Bike", newActivity.Name);
        Assert.Equal(cExercise, newActivity.Category.Id);
        Assert.Equal("Exercise", newActivity.Category.Name);
        Assert.Equal(expectedActivityCount, context.Activity.Count());
        Assert.Equal(expectedCategoryCount, context.Category.Count());
    }

    [Fact]
    public async Task ParseEntry_3ExerciseDoesNotExist_3BikeDoesNotExist_AddExerciseAndBike_CaseUser2()
    {
        var user = DbFixture.Users[2];
        int cExercise = 7;

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();
        var expectedActivityCount = context.Activity.Count() + 2;
        var expectedCategoryCount = context.Category.Count() + 1;

        // Act
        var sut = new EntryParserService(context);
        var res = await sut.ParseEntry(new List<string>()
        {
            "Exercise: Bike",
            "Exercise: Run",
        }, user);
        context.ChangeTracker.Clear();

        var cats = context.Category.ToList();

        // Assert
        var newActivity = context.Activity.Include(x => x.Category).Single(x => x.NormalizedName == "Exercise: Bike" && x.UserGuid == user);
        Assert.Equal("Exercise: Bike", newActivity.Name);
        Assert.Equal(cExercise, newActivity.Category.Id);
        Assert.Equal("Exercise", newActivity.Category.Name);

        newActivity = context.Activity.Include(x => x.Category).Single(x => x.NormalizedName == "Exercise: Run" && x.UserGuid == user);
        Assert.Equal("Exercise: Run", newActivity.Name);
        Assert.Equal(cExercise, newActivity.Category.Id);
        Assert.Equal("Exercise", newActivity.Category.Name);

        Assert.Equal(expectedActivityCount, context.Activity.Count());
        Assert.Equal(expectedCategoryCount, context.Category.Count());
    }

    [Fact]
    public async Task ParseEntry_Blank_DoNothing_CaseUser2()
    {
        var user = DbFixture.Users[2];

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();
        var expectedActivityCount = context.Activity.Count();
        var expectedCategoryCount = context.Category.Count();

        // Act
        var sut = new EntryParserService(context);
        var res = await sut.ParseEntry(new List<string>()
        {
        }, user);
        context.ChangeTracker.Clear();

        // Assert
        Assert.Equal(expectedActivityCount, context.Activity.Count());
        Assert.Equal(expectedCategoryCount, context.Category.Count());
    }

    [Fact]
    public async Task ParseEntry_NoDefaultCat_SwimDoesNotExist_AddDefaultCategory_CaseUser3()
    {
        var user = DbFixture.Users[3];

        // Arrange 
        using var context = DbFixture.CreateContext();
        var _dt = DbFixture.Dt;
        context.Database.BeginTransaction();
        var expectedCategoryCount = context.Category.Count() + 1;

        // Act
        var sut = new EntryParserService(context);
        var res = await sut.ParseEntry(new List<string>()
        {
            "Swim"
        }, user);
        context.ChangeTracker.Clear();

        // Assert
        var newActivity = context.Activity.Include(x => x.Category).Single(x => x.NormalizedName == "Swim" && x.UserGuid == user);
        var newCat = newActivity.Category;
        Assert.Equal("", newCat.Name);
        Assert.Equal(0, newCat.Priority);
        Assert.Equal(Color.White.R, newCat.Color.R);
        Assert.Equal(Color.White.G, newCat.Color.G);
        Assert.Equal(Color.White.B, newCat.Color.B);
        Assert.Equal(user, newCat.UserGuid);
        Assert.False(newCat.Deleted);
        Assert.Equal(expectedCategoryCount, context.Category.Count());
    }
}
