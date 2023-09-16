using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;
using WeeklyReview.Database.Persitance;

namespace WeeklyReview.Shared.Services
{
    internal class ActivityService : IActivityService
    {
        private readonly WeeklyReviewDbContext _db;
        private readonly IActivityChangeService _activityChangeService;

        public ActivityService(WeeklyReviewDbContext db, IActivityChangeService activityChangeService)
        {
            _db = db;
            _activityChangeService = activityChangeService;
        }

        public IEnumerable<ActivityModel> GetAll(Guid userGuid)
        {
            return _db.Activity.Where(x => x.UserGuid == userGuid);
        }

        public ActivityModel? Get(int key, Guid userGuid)
        {
            return _db.Activity.SingleOrDefault(x => x.Id == key && x.UserGuid == userGuid);
        }

        public ActivityModel Delete(int key, Guid userGuid)
        {
            var model = _db.Activity.SingleOrDefault(x => x.Id == key && x.UserGuid == userGuid);
            if (model is null)
                throw new KeyNotFoundException($"Model not found with id {key}");

            var entriesReferencesActivity = _db.Entry.Include(x => x.Activities).Any(x => x.Activities.Contains(model) && x.Deleted == false);
            if (entriesReferencesActivity)
                throw new InvalidOperationException ($"It is not possible to delete an activity which is still referenced by entries");

            model.Deleted = true;
            _db.SaveChanges();
            return model;
        }

        public ActivityChangeModel Convert(int sKey, int dKey, Guid userGuid)
        {
            return _activityChangeService.ChangeActivity(sKey, dKey, userGuid);
        }
    }
}
