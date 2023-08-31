using Microsoft.EntityFrameworkCore;
using Syncfusion.Blazor.Inputs;
using System.Diagnostics;
using WeeklyReview.Shared.Services;
using WeeklyReview.Shared.Tests.DataContexts;
using Xunit;

namespace WeeklyReview.Shared.Tests.Services
{
    public class NewEntryParserServiceTests : IClassFixture<WeeklyReviewApiDbFixtureForEntryParserService>
    {
        public WeeklyReviewApiDbFixtureForEntryParserService DbFixture { get; }

        public NewEntryParserServiceTests(WeeklyReviewApiDbFixtureForEntryParserService dbFixture)
        {
            DbFixture = dbFixture;
        }

        [Fact]
        public void ParseEntry_Swim_Exists_CaseUser1()
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
            var sut = new NewEntryParserService(context);
            var res = sut.ParseEntry(input, user);
            context.ChangeTracker.Clear();

            // Assert
            Assert.Single(res);
            Assert.Contains(res, x => x.Id == aSwim);
        }

        [Fact]
        public void ParseEntry_SwimWithSpace_Exists_CaseUser1()
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
            var sut = new NewEntryParserService(context);
            var res = sut.ParseEntry(input, user);
            context.ChangeTracker.Clear();

            // Assert
            Assert.Single(res);
            Assert.Contains(res, x => x.Id == aSwim);
        }

        [Fact]
        public void ParseEntry_SportsBike_Exists_CaseUser1()
        {
            int aBike = 3;
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
            var sut = new NewEntryParserService(context);
            var res = sut.ParseEntry(input, user);
            context.ChangeTracker.Clear();

            // Assert
            Assert.Single(res);
            Assert.Contains(res, x => x.Id == aBike);
        }

        [Fact]
        public void ParseEntry_SportsBikeAndDance_Exists_CaseUser1()
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
            var sut = new NewEntryParserService(context);
            var res = sut.ParseEntry(input, user);
            context.ChangeTracker.Clear();

            // Assert
            Assert.Equal(2, res.Count());
            Assert.Contains(res, x => x.Id == aBike);
            Assert.Contains(res, x => x.Id == aDance);
        }

        [Fact]
        public void ParseEntry_SportsBikeAndExerciseRun_Exists_CaseUser1()
        {
            int aBike = 3;
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
            var sut = new NewEntryParserService(context);
            var res = sut.ParseEntry(input, user);
            context.ChangeTracker.Clear();

            // Assert
            Assert.Equal(2, res.Count());
            Assert.Contains(res, x => x.Id == aBike);
            Assert.Contains(res, x => x.Id == aRun);
        }

        [Fact]
        public void ParseEntry_BlankAndSportsBike_Exists_CaseUser1()
        {
            int aBike = 3;
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
            var sut = new NewEntryParserService(context);
            var res = sut.ParseEntry(input, user);
            context.ChangeTracker.Clear();

            // Assert
            Assert.Single(res);
            Assert.Contains(res, x => x.Id == aBike);
        }

        [Fact]
        public void ParseEntry_DifficultCompetionsParticipatinginRaceWithSpaces_Exists_CaseUser1()
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
            var sut = new NewEntryParserService(context);
            var res = sut.ParseEntry(input, user);
            context.ChangeTracker.Clear();

            // Assert
            Assert.Single(res);
            Assert.Contains(res, x => x.Id == aRace);
        }

        [Theory]
        [InlineData(" ")]
        [InlineData(" \t ", " ")]
        public void ParseEntry_Blank_Exists_CaseUser1(params string[] input)
        {
            var user = DbFixture.Users[1];

            // Arrange 
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();

            // Act
            var sut = new NewEntryParserService(context);
            var res = sut.ParseEntry(input.ToList(), user);
            context.ChangeTracker.Clear();

            // Assert
            Assert.Empty(res);
        }

        [Fact]
        public void ParseEntry_NoEntries_Exists_CaseUser1()
        {
            var user = DbFixture.Users[1];

            // Arrange 
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();

            // Act
            var sut = new NewEntryParserService(context);
            var res = sut.ParseEntry(new List<string>(), user);
            context.ChangeTracker.Clear();

            // Assert
            Assert.Empty(res);
        }

        [Fact]
        public void ParseEntry_MultipleColons_Error_CaseUser1()
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
            var sut = new NewEntryParserService(context);

            // Assert
            var exception = Assert.Throws<ArgumentException>(() => sut.ParseEntry(input, user));
            context.ChangeTracker.Clear();
            Assert.Equal("An entry cannot contain multiple ':'", exception.Message);
        }

        [Fact]
        public void ParseEntry_OnlyCategory_Error_CaseUser1()
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
            var sut = new NewEntryParserService(context);

            // Assert
            var exception = Assert.Throws<ArgumentException>(() => sut.ParseEntry(input, user));
            context.ChangeTracker.Clear();
            Assert.Equal("An entry must contain an Activity, if a category is supplied", exception.Message);
        }

        [Fact]
        public void ParseEntry_DoubleSwimEntry_OnlyOneSwim_CaseUser1()
        {
            var user = DbFixture.Users[1];
            int aSwim = 1;

            // Arrange 
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();

            // Act
            var sut = new NewEntryParserService(context);
            var res = sut.ParseEntry(new List<string>()
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
        public void ParseEntry_ActivityExists_DoNothing_CaseUser2()
        {
            var user = DbFixture.Users[2];

            // Arrange 
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();
            var expectedActivityCount = context.Activity.Count();
            var expectedCategoryCount = context.Category.Count();

            // Act
            var sut = new NewEntryParserService(context);
            var res = sut.ParseEntry(new List<string>()
            {
                "Sports: Bike"
            }, user);
            context.ChangeTracker.Clear();

            // Assert
            Assert.Equal(expectedActivityCount, context.Activity.Count());
            Assert.Equal(expectedCategoryCount, context.Category.Count());
        }

        [Fact]
        public void ParseEntry_SportsExist_RunDoesNotExist_AddRun_CaseUser2()
        {
            var user = DbFixture.Users[2];
            int aRun = 7;
            int cSports = 4;

            // Arrange 
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();
            var expectedActivityCount = context.Activity.Count() + 1;
            var expectedCategoryCount = context.Category.Count();

            // Act
            var sut = new NewEntryParserService(context);
            var res = sut.ParseEntry(new List<string>()
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
        public void ParseEntry_ExerciseDoesNotExist_BikeDoesNotExist_AddExerciseAndBike_CaseUser2()
        {
            var user = DbFixture.Users[2];
            int aBike = 8;
            int cExercise = 5;

            // Arrange 
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();
            var expectedActivityCount = context.Activity.Count() + 1;
            var expectedCategoryCount = context.Category.Count() + 1;

            // Act
            var sut = new NewEntryParserService(context);
            var res = sut.ParseEntry(new List<string>()
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
        public void ParseEntry_Blank_DoNothing_CaseUser2()
        {
            var user = DbFixture.Users[2];

            // Arrange 
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();
            var expectedActivityCount = context.Activity.Count();
            var expectedCategoryCount = context.Category.Count();

            // Act
            var sut = new NewEntryParserService(context);
            var res = sut.ParseEntry(new List<string>()
            {
            }, user);
            context.ChangeTracker.Clear();

            // Assert
            Assert.Equal(expectedActivityCount, context.Activity.Count());
            Assert.Equal(expectedCategoryCount, context.Category.Count());
        }

    }
}
