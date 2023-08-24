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
    public class EntryAdderServiceTests
    {
        public WeeklyReviewApiDbFixtureForEntryAdderService DbFixture { get; }
        public ITimeService TimeService { get; }

        public EntryAdderServiceTests(WeeklyReviewApiDbFixtureForEntryAdderService dbFixture)
        {
            DbFixture = dbFixture;
            TimeService = Substitute.For<ITimeService>();
            TimeService.Current.Returns(dbFixture.MaxTime);

        }

        [Fact]
        public void Entry_OnlyEntry_AddEntry()
        {
            int aSeries = 1;
            int aMovies = 2;
            var date = DbFixture.Dt.AddHours(0);
            var user = DbFixture.Users[1];

            // Arrange 
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();

            // Act
            var sut = new NewEntryAdderService(context, TimeService);
            var activities = context.Activity.Where(x => x.Id == aSeries || x.Id == aMovies).ToList();
            sut.AddEntry(date, activities, user);
            context.ChangeTracker.Clear();

            // Assert
            var entry = context.Entry
                .Include(x => x.Activities)
                .Single(x => 
                    x.StartTime == date && 
                    x.UserGuid == user && 
                    x.Deleted == false);
            Assert.Null(entry.EndTime);
            Assert.Equal(2, entry.Activities.Count);
            Assert.Contains(activities[0], entry.Activities);
            Assert.Contains(activities[1], entry.Activities);
        }

    }
}
