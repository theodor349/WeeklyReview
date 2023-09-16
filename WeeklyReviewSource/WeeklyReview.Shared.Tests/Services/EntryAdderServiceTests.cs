using AutoFixture.Xunit2;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;
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
        public void Entry_OnlyEntry_AddEntry_CaseMovies()
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
            var sut = new EntryAdderService(context, TimeService);
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

        [Theory, AutoData]
        public void Entry_OnlyEntry_AddEntry_CaseMovies_CheckThatDateIsFlooredTo15Minutes([Range(0, 59)] int minutes)
        {
            int aSeries = 1;
            int aMovies = 2;
            var date = DbFixture.Dt.AddHours(0).AddMinutes(minutes);
            var expectedDate = DbFixture.Dt.AddHours(0).AddMinutes(minutes / 15 * 15);
            var entryDate = date.AddHours(1);
            var user = DbFixture.Users[1];

            // Arrange 
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();
            TimeService.Current.Returns(entryDate);

            // Act
            var sut = new EntryAdderService(context, TimeService);
            var activities = context.Activity.Where(x => x.Id == aSeries || x.Id == aMovies).ToList();
            var res = sut.AddEntry(date, activities, user);
            context.ChangeTracker.Clear();

            // Assert
            var entry = context.Entry
                .Include(x => x.Activities)
                .Single(x =>
                    x.StartTime == expectedDate &&
                    x.UserGuid == user &&
                    x.Deleted == false);
            Assert.Equivalent(entry, res);
            Assert.Equal(expectedDate.Minute, entry.StartTime.Minute);
        }

        [Fact]
        public void Entry_EntrieArround_AddEntryAndUpdateTimes_CaseFoods()
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
            var sut = new EntryAdderService(context, TimeService);
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

        [Fact]
        public void Entry_EntryAtAndAfter_OverrideEntry_CaseSports()
        {
            int aSwim = 8;
            var startTime = DbFixture.Dt.AddHours(0);
            var endTime = startTime.AddHours(4);
            var entryDate = startTime.AddHours(1);
            var user = DbFixture.Users[3];

            // Arrange 
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();
            TimeService.Current.Returns(entryDate);

            // Act
            var sut = new EntryAdderService(context, TimeService);
            var activities = context.Activity.Where(x => x.Id == aSwim).ToList();
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
            Assert.Contains(entry.Activities, x => x.Id == aSwim);

            var beforeDeleted = context.Entry.Single(x => x.StartTime == startTime && x.Deleted == true && x.UserGuid == user);
            Assert.True(beforeDeleted.Deleted);
        }

        [Fact]
        public void Entry_EntryAtBeforeAndAfter_InsertingBlank_DeleteEntryAndUpdateTime_CaseTrip()
        {
            var startTime = DbFixture.Dt.AddHours(2);
            var endTime = startTime.AddHours(2);
            var entryDate = startTime.AddHours(1);
            var user = DbFixture.Users[4];

            // Arrange 
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();
            TimeService.Current.Returns(entryDate);

            // Act
            var sut = new EntryAdderService(context, TimeService);
            var activities = new List<ActivityModel>();
            var res = sut.AddEntry(startTime, activities, user);
            context.ChangeTracker.Clear();

            // Assert
            var entry = context.Entry
                .Include(x => x.Activities)
                .SingleOrDefault(x =>
                    x.StartTime == startTime &&
                    x.UserGuid == user &&
                    x.Deleted == false);
            Assert.Null(entry);
            Assert.Null(res);

            var before = context.Entry.Single(x => x.StartTime == startTime.AddHours(-2) && x.Deleted == false && x.UserGuid == user);
            Assert.Equal(endTime, before.EndTime);
        }
    }
}
