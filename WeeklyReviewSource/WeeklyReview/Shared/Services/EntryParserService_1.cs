using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;
using WeeklyReview.Database.Persitance;

namespace WeeklyReview.Shared.Services
{
    public class NewEntryParserService
    {
        private class ActivityCategory
        {
            public string? Activity { get; set; }
            public string? Category { get; set; }

            public ActivityCategory(string? activity, string? category)
            {
                Activity = activity;
                Category = category;
            }
        }

        private readonly WeeklyReviewDbContext _db;

        public NewEntryParserService(WeeklyReviewDbContext db)
        {
            _db = db;
        }

        public List<ActivityModel> ParseEntry(List<string> activities, Guid userGuid)
        {
            var activityCategories = GetActivityCategories(activities);
            var res = AddActivityCategories(activityCategories, userGuid);
            return res;
        }

        private List<ActivityModel> AddActivityCategories(List<ActivityCategory> entires, Guid userGuid)
        {
            // Ensure all Categories and Activities exist
            // Retrive relvant Activities
            var res = new List<ActivityModel>();

            foreach (var entry in entires)
            {
                var act = _db.Activity.Single(x => x.NormalizedName == entry.Activity.ToLower() && x.UserGuid == userGuid);
                res.Add(act);
            }

            return res;
        }

        private List<ActivityCategory> GetActivityCategories(List<string> entries)
        {
            var res = new List<ActivityCategory>();
            foreach (var entry in entries)
            {
                var parseResult = ParseEntry(entry);
                if (parseResult.Activity is not null)
                    res.Add(parseResult);
            }
            return res;
        }

        private ActivityCategory ParseEntry(string entry)
        {
            var splits = entry.Split(':');
            string? cat = null;
            string? act;
            if (splits.Count() > 2)
            {
                throw new ArgumentException("An entry cannot contain multiple ':'");
            }
            else if (splits.Count() == 2)
            {
                cat = splits[0];
                act = splits[1];
            }
            else
            {
                act = splits[0];
            }

            return new ActivityCategory(TrimSentance(act), TrimSentance(cat));
        }

        private string? TrimSentance(string? sentance)
        {
            if (sentance is null)
                return null;

            var regX = new Regex("\\s");
            var res = regX.Replace(sentance, "");
            if (res.Count() > 0)
                return res;
            else
                return null;
        }
    }
}
