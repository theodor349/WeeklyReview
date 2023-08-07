using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;

namespace WeeklyReview.Shared.Services
{
    internal class ActivityRollBackService : IActivityRollBackService
    {
        private readonly IDataService _dataService;

        public ActivityRollBackService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public Task RollBackActivityChange(ActivityChangeModel activityChange)
        {
            throw new NotImplementedException();
        }
    }
}
