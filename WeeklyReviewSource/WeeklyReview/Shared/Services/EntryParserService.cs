using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeeklyReview.Shared.Models;

namespace WeeklyReview.Shared.Services
{
    public class EntryParserService : IEntryParserService
    {
        public (List<Activity> usedActivities, List<Category> usedCategories, List<Activity> newActivities, List<Category> newCategories)
            ParseEntries(List<string> entries, List<Activity> activities, List<Category> categories, Category defaultCategory)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);

            var usedActivities = new List<Activity>();
            var usedCategories = new List<Category>();

            var newActivities = new List<Activity>();
            var newCategories = new List<Category>();
            foreach (var entry in entries)
            {
                var trimmedEntry = regex.Replace(entry, " ").Trim();
                var splits = trimmedEntry.Split(":");
                var activity = trimmedEntry;
                var category = splits.Length > 1 ? splits[0] : "";

                var a = activities.FirstOrDefault(x => x.Name == activity);
                if (a is null)
                {
                    a = new Activity(activity, defaultCategory, false);
                    newActivities.Add(a);
                }
                usedActivities.Add(a);

                var c = categories.FirstOrDefault(x => x.Name == category);
                if (c is null)
                {
                    c = new Category(splits[0], 0, Color.White);
                    newCategories.Add(c);
                }
                usedCategories.Add(c);
            }

            return (usedActivities, usedCategories, newActivities, newCategories);
        }
    }
}
