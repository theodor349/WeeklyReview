using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeeklyReview.Shared.Models;

namespace WeeklyReview.Shared.Services
{
    public class EntryParserService : IEntryParserService
    {
        public (List<Activity> usedActivities, List<Category> usedCategories, List<Activity> newActivities, List<Category> newCategories)
            ParseEntries(IEnumerable<string> entries, IEnumerable<Activity> activities, IEnumerable<Category> categories, Category defaultCategory)
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

                var c = category.Length == 0 ? defaultCategory : categories.FirstOrDefault(x => x.Name == category);
                if (c is null)
                {
                    c = new Category(splits[0], 0, Color.White);
                    newCategories.Add(c);
                }

                var a = activities.FirstOrDefault(x => x.Name == activity);
                if (a is null)
                {
                    a = new Activity(activity, c, false);
                    newActivities.Add(a);
                }
                usedActivities.Add(a);
                usedCategories.Add(c);
            }

            return (usedActivities, usedCategories, newActivities, newCategories);
        }
    }
}
