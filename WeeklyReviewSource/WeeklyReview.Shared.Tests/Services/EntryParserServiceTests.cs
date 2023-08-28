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
            int aBike = 4;
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
            int aBike = 4;
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
            int aBike = 4;
            int aRun = 5;
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
            int aBike = 4;
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
            int aRace = 6;

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

    }
}
