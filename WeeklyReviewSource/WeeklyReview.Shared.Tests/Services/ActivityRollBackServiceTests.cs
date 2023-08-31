using Microsoft.EntityFrameworkCore;
using NSubstitute;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;
using WeeklyReview.Database.Persitance;
using WeeklyReview.Shared.Services;
using WeeklyReview.Shared.Tests.DataContexts;
using Xunit;

namespace WeeklyReview.Shared.Tests.Services
{
    /// <summary> Notes
    /// Should we delete the Destination activity?
    ///     - No, because we might not have changed all entries that reference the Destination activity
    ///     - Therefore deletion should be another service which also checks that no enties references it
    /// </summary>

    public class ActivityRollBackServiceTests : IClassFixture<WeeklyReviewApiDbFixtureForActivityRollbackService>
    {
        public WeeklyReviewApiDbFixtureForActivityRollbackService DbFixture { get; }
        public ITimeService TimeService { get; }
        
        public ActivityRollBackServiceTests(WeeklyReviewApiDbFixtureForActivityRollbackService dbFixture)
        {
            DbFixture = dbFixture;
            TimeService = Substitute.For<ITimeService>();
            TimeService.Current.Returns(dbFixture.MaxTime);
        }

        #region Helpers
        private static void CheckEntryHaveBeenMarkedDeleted(int eOldId, WeeklyReviewDbContext context)
        {
            Assert.True(context.Entry.Single(x => x.Id == eOldId).Deleted);
        }

        private void CheckActivityChangeHaveBeenRemoved(WeeklyReviewDbContext context, int changeId)
        {
            Assert.False(context.ActivityChange.Any(x => x.Id == changeId));
        }

        private static EntryModel CheckEntryIsEnabledWithCorrectActivities(WeeklyReviewDbContext context, DateTime startTime, DateTime endTime, List<int> activityIds, Guid userGuid)
        {
            var entry = context.Entry
                .Include(x => x.Activities)
                .Single(x => x.StartTime == startTime && x.Deleted == false);
            Assert.Equal(endTime, entry.EndTime);
            Assert.Equal(userGuid, entry.UserGuid);
            Assert.Equal(activityIds.Count(), entry.Activities.Count());
            foreach (var aId in activityIds)
                Assert.Contains(entry.Activities, x => x.Id == aId);
            return entry;
        }

        private void CheckThatActivityIsEnabled()
        {
        }
        #endregion

        /// <summary>
        /// TODO: Fix Test
        ///     Seems like the dabase is inserting the activities in a non-determanistic fassion, e.g. Spain is Id=0 but should be Id=14
        /// </summary>

        [Fact]
        public void Rolback_NoNewEntry_Override_CaseMovies()
        {
            int changeId = 1;
            int eOldId = 2;
            int aMovie = 1;
            var startTime = DbFixture.Dt.AddHours(0);
            var endTime = startTime.AddHours(1);
            var userGuid = DbFixture.User1;

            // Arrange
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();

            // Act
            var sut = new ActivityRollBackService(context, TimeService);
            var model = context.ActivityChange
                .Include(x => x.Source)
                .Include(x => x.Destination)
                .Single(x => x.Id == changeId);
            sut.RollBackActivityChange(model);
            context.ChangeTracker.Clear();

            // Assert
            var newEntry = CheckEntryIsEnabledWithCorrectActivities(context, startTime, endTime, new() { aMovie }, userGuid);
            Assert.Equal(TimeService.Current, newEntry.RecordedTime);
            Assert.False(context.Activity.Single(x => x.Id == aMovie).Deleted);
            CheckEntryHaveBeenMarkedDeleted(eOldId, context);
            CheckActivityChangeHaveBeenRemoved(context, changeId);
        }

        [Fact]
        public void Rolback_NewEntryWithSameActivityAndAnother_Override_CaseSports()
        {
            int changeId = 2;
            int eOldId = 5;
            int aBike = 3;
            int aRun = 5;
            var startTime = DbFixture.Dt.AddHours(2);
            var endTime = startTime.AddHours(1);
            var userGuid = DbFixture.User1;

            // Arrange
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();

            // Act
            var sut = new ActivityRollBackService(context, TimeService);
            var model = context.ActivityChange
                .Include(x => x.Source)
                .Include(x => x.Destination)
                .Single(x => x.Id == changeId);
            sut.RollBackActivityChange(model);
            context.ChangeTracker.Clear();

            // Assert

            var newEntry = CheckEntryIsEnabledWithCorrectActivities(context, startTime, endTime, new() { aBike, aRun }, userGuid);
            Assert.Equal(TimeService.Current, newEntry.RecordedTime);
            CheckEntryHaveBeenMarkedDeleted(eOldId, context); 
            CheckActivityChangeHaveBeenRemoved(context, changeId);
        }

        [Fact]
        public void Rolback_NewEntryWithoutSameActivityAndAnother_DoNotOverride_CaseFoods()
        {
            int changeId = 3;
            int aDinner = 8;
            int aSnack = 9;
            var startTime = DbFixture.Dt.AddHours(4);
            var endTime = startTime.AddHours(1);
            var userGuid = DbFixture.User1;

            // Arrange
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();
            var expectedEntryCount = context.Entry.Count();

            // Act
            var sut = new ActivityRollBackService(context, TimeService);
            var model = context.ActivityChange
                .Include(x => x.Source)
                .Include(x => x.Destination)
                .Single(x => x.Id == changeId);
            sut.RollBackActivityChange(model);
            context.ChangeTracker.Clear();

            // Assert
            CheckEntryIsEnabledWithCorrectActivities(context, startTime, endTime, new() { aDinner, aSnack }, userGuid);
            CheckActivityChangeHaveBeenRemoved(context, changeId);
            Assert.Equal(expectedEntryCount, context.Entry.Count());
        }

        [Fact]
        public void Rolback_NewEntryWithoutSameActivityAndNewEntryWithSameActivity_DoNotOverride_CaseSchool()
        {
            int changeId = 4;
            int aEnglish = 11;
            var startTime = DbFixture.Dt.AddHours(6);
            var endTime = startTime.AddHours(1);
            var userGuid = DbFixture.User1;

            // Arrange
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();
            var expectedEntryCount = context.Entry.Count();

            // Act
            var sut = new ActivityRollBackService(context, TimeService);
            var model = context.ActivityChange
                .Include(x => x.Source)
                .Include(x => x.Destination)
                .Single(x => x.Id == changeId);
            sut.RollBackActivityChange(model);
            context.ChangeTracker.Clear();

            // Assert
            CheckEntryIsEnabledWithCorrectActivities(context, startTime, endTime, new() { aEnglish }, userGuid);
            CheckActivityChangeHaveBeenRemoved(context, changeId);
            Assert.Equal(expectedEntryCount, context.Entry.Count());
        }

        [Fact]
        public void Rolback_NewEntryHaveBeenDeleted_DoNothing_CaseTrip()
        {
            int changeId = 5;
            int aSpain = 14;
            var startTime = DbFixture.Dt.AddHours(8);
            var endTime = startTime.AddHours(1);
            var userGuid = DbFixture.User1;

            // Arrange
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();
            var expectedEntryCount = context.Entry.Count();

            // Act
            var sut = new ActivityRollBackService(context, TimeService);
            var model = context.ActivityChange
                .Include(x => x.Source)
                .Include(x => x.Destination)
                .Single(x => x.Id == changeId);
            sut.RollBackActivityChange(model);
            context.ChangeTracker.Clear();

            // Assert
            var entry = context.Entry.SingleOrDefault(x => x.StartTime == startTime && x.UserGuid == userGuid && x.Deleted == false);
            Assert.Null(entry);
            var activity = context.Activity.Single(x => x.Id == aSpain);
            Assert.False(activity.Deleted);
            CheckActivityChangeHaveBeenRemoved(context, changeId);
            Assert.Equal(expectedEntryCount, context.Entry.Count());
        }
    }
}
