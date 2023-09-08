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
    public class ActivityChangeServiceTests : IClassFixture<WeeklyReviewApiDbFixtureForActivityChangeService>
    {
        public WeeklyReviewApiDbFixtureForActivityChangeService DbFixture { get; }
        public ITimeService TimeService { get; }

        public ActivityChangeServiceTests(WeeklyReviewApiDbFixtureForActivityChangeService dbFixture)
        {
            DbFixture = dbFixture;
            TimeService = Substitute.For<ITimeService>();
            TimeService.Current.Returns(dbFixture.MaxTime);
        }

        [Fact]
        public void Change_NoEntry_DoNothing_CaseFoods()
        {
            var userGuid = DbFixture.User1;
            var aLunch = 1;
            var aDinner = 2;

            // Arrange
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();
            int expectedEntryCount = context.Entry.Count();

            // Act
            var sut = new ActivityChangeService(context, TimeService);
            sut.ChangeActivity(
                context.Activity.Single(x => x.Id == aLunch), 
                context.Activity.Single(x => x.Id == aDinner),
                userGuid);
            context.ChangeTracker.Clear();

            // Assert
            Assert.Equal(expectedEntryCount, context.Entry.Count());
            var lunch = context.Activity.Single(x => x.Id == aLunch);
            Assert.True(lunch.Deleted);
            var dinner = context.Activity.Single(x => x.Id == aDinner);
            Assert.False(dinner.Deleted);
        }

        [Fact]
        public void Change_NoEntry_AddChange_CaseFoods()
        {
            var userGuid = DbFixture.User1;
            var aLunch = 1;
            var aDinner = 2;

            // Arrange
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();
            var expectedChangeDate = DateTime.Now;
            TimeService.Current.Returns(expectedChangeDate);

            // Act
            var sut = new ActivityChangeService(context, TimeService);
            var res = sut.ChangeActivity(
                context.Activity.Single(x => x.Id == aLunch),
                context.Activity.Single(x => x.Id == aDinner),
                userGuid);
            context.ChangeTracker.Clear();

            // Assert
            var change = context.ActivityChange
                .Include(x => x.Source)
                .Include(x => x.Destination)
                .Single(x => x.Source.Id == aLunch && x.Destination.Id == aDinner && x.UserGuid == userGuid);
            Assert.Equal(expectedChangeDate, change.ChangeDate);
            Assert.Equal(res.Id, change.Id);
            Assert.Equivalent(res, change);
        }

        [Fact]
        public void Change_MultipleEntries_OverrideAll_CaseSports()
        {
            var userGuid = DbFixture.User1;
            var aRun = 3;
            var aBike = 4;
            var e1StartTime = DbFixture.Dt.AddHours(1);
            var e2StartTime = e1StartTime.AddHours(2);

            // Arrange
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();
            int expectedEntryCount = context.Entry.Count() + 2;

            // Act
            var sut = new ActivityChangeService(context, TimeService);
            sut.ChangeActivity(
                context.Activity.Single(x => x.Id == aRun),
                context.Activity.Single(x => x.Id == aBike),
                userGuid);
            context.ChangeTracker.Clear();

            // Assert
            Assert.Equal(expectedEntryCount, context.Entry.Count());
            // Old
            var e1 = context.Entry.Include(x => x.Activities).Single(x => x.StartTime == e1StartTime && x.Deleted == true && x.UserGuid == userGuid);
            Assert.True(e1.Deleted);
            var e2 = context.Entry.Include(x => x.Activities).Single(x => x.StartTime == e2StartTime && x.Deleted == true && x.UserGuid == userGuid);
            Assert.True(e2.Deleted);
            // new
            e1 = context.Entry.Include(x => x.Activities).Single(x => x.StartTime == e1StartTime && x.Deleted == false && x.UserGuid == userGuid);
            Assert.Single(e1.Activities);
            Assert.Contains(e1.Activities, x => x.Id == aBike);
            Assert.False(e1.Deleted);
            Assert.Equal(e1StartTime.AddHours(1), e1.EndTime);
            Assert.Equal(e1StartTime.AddMinutes(1), e1.RecordedTime);
            e2 = context.Entry.Include(x => x.Activities).Single(x => x.StartTime == e2StartTime && x.Deleted == false && x.UserGuid == userGuid);
            Assert.Single(e2.Activities);
            Assert.Contains(e2.Activities, x => x.Id == aBike);
            Assert.False(e2.Deleted);
            Assert.Equal(e2StartTime.AddHours(1), e2.EndTime);
            Assert.Equal(e2StartTime.AddMinutes(1), e2.RecordedTime);
        }

        [Fact]
        public void Change_MultipleActivities_OverrideCorrect_CaseTravel()
        {
            var userGuid = DbFixture.User1;
            var aSpain = 5;
            var aItaly = 6;
            var aFrance = 7;
            var startTime = DbFixture.Dt.AddHours(6);

            // Arrange
            using var context = DbFixture.CreateContext();
            var _dt = DbFixture.Dt;
            context.Database.BeginTransaction();
            int expectedEntryCount = context.Entry.Count() + 2;

            // Act
            var sut = new ActivityChangeService(context, TimeService);
            sut.ChangeActivity(
                context.Activity.Single(x => x.Id == aSpain),
                context.Activity.Single(x => x.Id == aFrance),
                userGuid);
            context.ChangeTracker.Clear();

            // Assert
            var e = context.Entry.Include(x => x.Activities).Single(x => x.StartTime == startTime && x.Deleted == false && x.UserGuid == userGuid);
            Assert.Equivalent(2, e.Activities.Count());
            Assert.Contains(e.Activities, x => x.Id == aItaly);
            Assert.Contains(e.Activities, x => x.Id == aFrance);
        }
    }
}
