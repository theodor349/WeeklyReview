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
    public class EntryAdderServiceTests : IClassFixture<WeeklyReviewApiDbFixtureForEntryAdderService>
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
            var entryDate = date.AddHours(1);
            var user = DbFixture.Users[1];

            // Arrange 
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();
            TimeService.Current.Returns(entryDate);

            // Act
            var sut = new NewEntryAdderService(context, TimeService);
            var activities = context.Activity.Where(x => x.Id == aSeries || x.Id == aMovies).ToList();
            var res = sut.AddEntry(date, activities, user);
            context.ChangeTracker.Clear();

            // Assert
            var entry = context.Entry
                .Include(x => x.Activities)
                .Single(x =>
                    x.StartTime == date &&
                    x.UserGuid == user &&
                    x.Deleted == false);
            Assert.Equivalent(entry, res);
            Assert.Null(entry.EndTime);
            Assert.Equal(entryDate, entry.RecordedTime);
            Assert.Equal(2, entry.Activities.Count);
            Assert.Contains(entry.Activities, x => x.Id == aSeries);
            Assert.Contains(entry.Activities, x => x.Id == aMovies);
        }

        [Fact]
        public void Entry_EntrieArround_AddEntryAndUpdateTimes()
        {
            int aDinner = 5;
            var startTime = DbFixture.Dt.AddHours(2);
            var endTime = startTime.AddHours(2);
            var entryDate = startTime.AddHours(1);
            var user = DbFixture.Users[2];

            // Arrange 
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();
            TimeService.Current.Returns(entryDate);

            // Act
            var sut = new NewEntryAdderService(context, TimeService);
            var activities = context.Activity.Where(x => x.Id == aDinner).ToList();
            var res = sut.AddEntry(startTime, activities, user);
            context.ChangeTracker.Clear();

            // Assert
            var entry = context.Entry
                .Include(x => x.Activities)
                .Single(x =>
                    x.StartTime == startTime &&
                    x.UserGuid == user &&
                    x.Deleted == false);
            Assert.Equivalent(entry, res);
            Assert.Equal(endTime, entry.EndTime);
            Assert.Equal(entryDate, entry.RecordedTime);
            Assert.Single(entry.Activities);
            Assert.Contains(entry.Activities, x => x.Id == aDinner);

            var beforeDeleted = context.Entry.Single(x => x.StartTime == startTime.AddHours(-2) && x.Deleted == true && x.UserGuid == user);
            var beforeActive = context.Entry.Single(x => x.StartTime == startTime.AddHours(-2) && x.Deleted == false && x.UserGuid == user);
            Assert.Equal(endTime, beforeDeleted.EndTime);
            Assert.Equal(startTime, beforeActive.EndTime);
        }

    }
}
