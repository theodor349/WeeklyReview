using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Shared.Services;
using WeeklyReview.Shared.Tests.DataContexts;
using Xunit;

namespace WeeklyReview.Shared.Tests.Services
{
    public class EntryServiceTests : IClassFixture<WeeklyReviewApiDbFixtureForEntryService>
    {
        private IEntryAdderService _entryAdderService;
        private IEntryParserService _entryParserService;

        public WeeklyReviewApiDbFixtureForEntryService DbFixture { get; }
        public ITimeService TimeService { get; }

        public EntryServiceTests(WeeklyReviewApiDbFixtureForEntryService dbFixture)
        {
            DbFixture = dbFixture;
            TimeService = Substitute.For<ITimeService>();
            TimeService.Current.Returns(dbFixture.MaxTime);

            _entryAdderService = Substitute.For<IEntryAdderService>();
            _entryParserService = Substitute.For<IEntryParserService>();

        }

        [Fact]
        public async void Entry_Day10_PM5_CheckActivitiesAndCategories()
        {
            var date = DbFixture.Dt.AddHours(0);
            var entryDate = date.AddDays(10);
            var user = DbFixture.Users[2];
            int daysAround = 5;
            int expectedCount = 11;

            // Arrange 
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();

            // Act
            var sut = new EntryService(context, _entryAdderService, _entryParserService);
            var res = await sut.GetAllAroundDate(user, entryDate, daysAround);
            context.ChangeTracker.Clear();

            // Assert
            Assert.Equal(expectedCount, res.Count());
            foreach (var entry in res)
            {
                Assert.NotNull(entry.Activities);
                var act = entry.Activities.FirstOrDefault();
                Assert.NotNull(act);
                var cat = act.Category;
                Assert.NotNull(cat);
            }
        }

        [Fact]
        public async void Entry_Day10_PM5_Get10()
        {
            var date = DbFixture.Dt.AddHours(0);
            var entryDate = date.AddDays(10);
            var user = DbFixture.Users[1];
            int daysAround = 5;
            int expectedCount = 11;

            // Arrange 
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();

            // Act
            var sut = new EntryService(context, _entryAdderService, _entryParserService);
            var res = await sut.GetAllAroundDate(user, entryDate, daysAround);
            context.ChangeTracker.Clear();

            // Assert
            Assert.Equal(expectedCount, res.Count());
            foreach (var entry in res)
            {
                var time = entry.StartTime - date;
                Assert.True(time.TotalDays >= 5);
                Assert.True(time.TotalDays <= 15);
            }
        }
    }
}
