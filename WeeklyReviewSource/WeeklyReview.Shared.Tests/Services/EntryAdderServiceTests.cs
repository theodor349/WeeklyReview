using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Shared.Services;
using WeeklyReview.Shared.Tests.DataContexts;

namespace WeeklyReview.Shared.Tests.Services
{
    public class EntryAdderServiceTests
    {
        public WeeklyReviewApiDbFixtureForActivityRollbackService DbFixture { get; }
        public ITimeService TimeService { get; }

        public EntryAdderServiceTests(WeeklyReviewApiDbFixtureForActivityRollbackService dbFixture)
        {
            DbFixture = dbFixture;
            TimeService = Substitute.For<ITimeService>();
            TimeService.Current.Returns(dbFixture.MaxTime);
        }
    }
}
