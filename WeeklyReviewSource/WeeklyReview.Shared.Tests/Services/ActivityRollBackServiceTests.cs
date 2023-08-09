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

        // Case: No new Entry has been added after the change
        [Fact]
        public void Rolback_NoNewEntry_AddOldEntry()
        {
            // Arrange
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();

            // Act
            var sut = new ActivityRollBackService(context);
            var model = context.ActivityChange
                .Include(x => x.Source)
                .Include(x => x.Destination)
                .Single(x => x.Id == 1);
            sut.RollBackActivityChange(model);
            context.ChangeTracker.Clear();

            // Assert
            var newEntry = context.Entry
                .Include(x => x.Activities)
                .Single(x => x.StartTime == _dt.AddHours(2) && x.Deleted == false);
            Assert.True(context.Entry.Single(x => x.Id == 6).Deleted);
            Assert.Equal(_dt.AddHours(2), newEntry.StartTime);
            Assert.Equal(_dt.AddHours(3), newEntry.EndTime);
            Assert.Equal(2, newEntry.Activities.Count());
            Assert.Contains(newEntry.Activities, x => x.Id == 3);
            Assert.Contains(newEntry.Activities, x => x.Id == 4);
            Assert.Equal(0, context.ActivityChange.Count());
        }

        // Case: A new Entry has been added, but where the changed activity is used again
        [Fact]
        public void Rolback_NewEntryWithSameOldActivity_AddOldEntry()
        {
            // Arrange
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();

            context.Entry.Single(x => x.Id == 9).Deleted = true;
            context.Entry.Add(new EntryModel(0, _dt.AddHours(3), _dt.AddHours(4), new List<ActivityModel>() 
            { 
                context.Activity.Single(x => x.Id == 5), context.Activity.Single(x => x.Id == 2) 
            }, false));
            context.SaveChanges();

            // Act
            var sut = new ActivityRollBackService(context);
            var model = context.ActivityChange
                .Include(x => x.Source)
                .Include(x => x.Destination)
                .Single(x => x.Id == 1);
            sut.RollBackActivityChange(model);
            context.ChangeTracker.Clear();

            // Assert
            var newEntry = context.Entry
                .Include(x => x.Activities)
                .Single(x => x.StartTime == _dt.AddHours(3) && x.Deleted == false);
            Assert.True(context.Entry.Single(x => x.Id == 9).Deleted);
            Assert.Equal(_dt.AddHours(3), newEntry.StartTime);
            Assert.Equal(_dt.AddHours(4), newEntry.EndTime);
            Assert.Equal(2, newEntry.Activities.Count());
            Assert.Contains(newEntry.Activities, x => x.Id == 3);
            Assert.Contains(newEntry.Activities, x => x.Id == 2);
            Assert.Equal(0, context.ActivityChange.Count());
        }

        // Case: A new Entry has been added, but where the changed activity is not used again
        [Fact]
        public void Rolback_NewEntryWithoutSameOldActivity_DoNotAddNewEntry()
        {
            // Arrange
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();

            // Act
            var sut = new ActivityRollBackService(context);
            var model = context.ActivityChange
                .Include(x => x.Source)
                .Include(x => x.Destination)
                .Single(x => x.Id == 1);
            sut.RollBackActivityChange(model);
            context.ChangeTracker.Clear();

            // Assert
            Assert.False(context.Entry.Single(x => x.Id == 8).Deleted);
            Assert.Equal(9, context.Entry.Count());
            Assert.Equal(0, context.ActivityChange.Count());
        }

        // Case: A new Entry has been added, but where the changed activity is not used again
        [Fact]
        public void Rolback_NewEntryWithAndWithoutSameOldActivity_DoNotAddNewEntry()
        {
            // Arrange
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();

            context.Entry.Single(x => x.Id == 7).Deleted = true;
            context.Entry.Add(new EntryModel(0, _dt.AddHours(3), _dt.AddHours(4), new List<ActivityModel>() { context.Activity.Single(x => x.Id == 5), context.Activity.Single(x => x.Id == 2) }, true));
            context.Entry.Add(new EntryModel(0, _dt.AddHours(3), _dt.AddHours(4), new List<ActivityModel>() { context.Activity.Single(x => x.Id == 1) }, true));
            context.Entry.Add(new EntryModel(0, _dt.AddHours(3), _dt.AddHours(4), new List<ActivityModel>() { context.Activity.Single(x => x.Id == 5) }, false));
            context.SaveChanges();

            // Act
            var sut = new ActivityRollBackService(context);
            var model = context.ActivityChange
                .Include(x => x.Source)
                .Include(x => x.Destination)
                .Single(x => x.Id == 1);
            sut.RollBackActivityChange(model);
            context.ChangeTracker.Clear();

            // Assert
            Assert.False(context.Entry.Single(x => x.Id == 11).Deleted);
            Assert.Equal(11, context.Entry.Count());
            Assert.Equal(0, context.ActivityChange.Count());
        }
    }
}
