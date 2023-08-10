using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;
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

    public class ActivityRollBackServiceTests : IClassFixture<WeeklyReviewApiDbFixture>
    {
        public WeeklyReviewApiDbFixture DbFixture { get; }
        
        public ActivityRollBackServiceTests(WeeklyReviewApiDbFixture dbFixture)
        {
            DbFixture = dbFixture;
        }

        // TODO: Implemente Tests
        //  - Old activity is not turned off
	    //  - The enty's duration/endtime has changed

        [Fact]
        public void Rolback_NoNewEntry_Override_CaseMovies()
        {
            int changeId = 1;
            int eOldId = 2;
            int aMovie = 1;
            var startTime = DbFixture.Dt.AddHours(0);
            var endTime = startTime.AddHours(1);

            // Arrange
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();

            // Act
            var sut = new ActivityRollBackService(context);
            var model = context.ActivityChange
                .Include(x => x.Source)
                .Include(x => x.Destination)
                .Single(x => x.Id == changeId);
            sut.RollBackActivityChange(model);
            context.ChangeTracker.Clear();

            // Assert
            var newEntry = context.Entry
                .Include(x => x.Activities)
                .Single(x => x.StartTime == startTime && x.Deleted == false);
            Assert.True(context.Entry.Single(x => x.Id == eOldId).Deleted);
            Assert.False(newEntry.Deleted);
            Assert.Equal(startTime, newEntry.StartTime);
            Assert.Equal(endTime, newEntry.EndTime);
            Assert.Single(newEntry.Activities);
            Assert.Contains(newEntry.Activities, x => x.Id == aMovie);
            Assert.False(context.ActivityChange.Any(x => x.Id == changeId));
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

            // Arrange
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();

            // Act
            var sut = new ActivityRollBackService(context);
            var model = context.ActivityChange
                .Include(x => x.Source)
                .Include(x => x.Destination)
                .Single(x => x.Id == changeId);
            sut.RollBackActivityChange(model);
            context.ChangeTracker.Clear();

            // Assert
            var newEntry = context.Entry
                .Include(x => x.Activities)
                .Single(x => x.StartTime == startTime && x.Deleted == false);
            Assert.True(context.Entry.Single(x => x.Id == eOldId).Deleted);
            Assert.False(newEntry.Deleted);
            Assert.Equal(startTime, newEntry.StartTime);
            Assert.Equal(endTime, newEntry.EndTime);
            Assert.Equal(2, newEntry.Activities.Count());
            Assert.Contains(newEntry.Activities, x => x.Id == aBike);
            Assert.Contains(newEntry.Activities, x => x.Id == aRun);
            Assert.False(context.ActivityChange.Any(x => x.Id == changeId));
        }

        [Fact]
        public void Rolback_NewEntryWithoutSameActivityAndAnother_DoNotOverride_CaseFoods()
        {
            int changeId = 3;
            int aDinner = 8;
            int aSnack = 9;
            var startTime = DbFixture.Dt.AddHours(4);
            var endTime = startTime.AddHours(1);

            // Arrange
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();
            var expectedEntryCount = context.Entry.Count();

            // Act
            var sut = new ActivityRollBackService(context);
            var model = context.ActivityChange
                .Include(x => x.Source)
                .Include(x => x.Destination)
                .Single(x => x.Id == changeId);
            sut.RollBackActivityChange(model);
            context.ChangeTracker.Clear();

            // Assert
            var oldEntry = context.Entry
                .Include(x => x.Activities)
                .Single(x => x.StartTime == startTime && x.Deleted == false);
            Assert.Contains(oldEntry.Activities, x => x.Id == aDinner);
            Assert.Contains(oldEntry.Activities, x => x.Id == aSnack);
            Assert.False(context.ActivityChange.Any(x => x.Id == changeId));
            Assert.Equal(expectedEntryCount, context.Entry.Count());
        }

        [Fact]
        public void Rolback_NewEntryWithoutSameActivityAndNewEntryWithSameActivity_DoNotOverride_CaseSchool()
        {
            int changeId = 4;
            int aEnglish = 11;
            var startTime = DbFixture.Dt.AddHours(6);
            var endTime = startTime.AddHours(1);

            // Arrange
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();
            var expectedEntryCount = context.Entry.Count();

            // Act
            var sut = new ActivityRollBackService(context);
            var model = context.ActivityChange
                .Include(x => x.Source)
                .Include(x => x.Destination)
                .Single(x => x.Id == changeId);
            sut.RollBackActivityChange(model);
            context.ChangeTracker.Clear();

            // Assert
            var oldEntry = context.Entry
                .Include(x => x.Activities)
                .Single(x => x.StartTime == startTime && x.Deleted == false);
            Assert.Single(oldEntry.Activities);
            Assert.Contains(oldEntry.Activities, x => x.Id == aEnglish);
            Assert.False(context.ActivityChange.Any(x => x.Id == changeId));
            Assert.Equal(expectedEntryCount, context.Entry.Count());
        }
    }
}
