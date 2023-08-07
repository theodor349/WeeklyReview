using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;
using WeeklyReview.Database.Persitance;

namespace WeeklyReview.Shared.Services
{
    internal class ActivityRollBackService : IActivityRollBackService
    {
        private readonly WeeklyReviewDbContext _dataService;

        public ActivityRollBackService(WeeklyReviewDbContext dataService)
        {
            _dataService = dataService;
        }

        public Task RollBackActivityChange(ActivityChangeModel activityChange)
        {
            throw new NotImplementedException();
        }
    }
}
