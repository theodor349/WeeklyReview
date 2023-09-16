using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WeeklyReview.Shared.Models.DTOs;

namespace WeeklyReview.Shared.Services
{
    internal class EntryParserService : IEntryParserService
    {
        public (List<ActivityDto> usedActivities, List<CategoryDto> usedCategories, List<ActivityDto> newActivities, List<CategoryDto> newCategories)
            ParseEntries(IEnumerable<string> entries, IEnumerable<ActivityDto> activities, IEnumerable<CategoryDto> categories, CategoryDto defaultCategory)
        {
            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);

            var usedActivities = new List<ActivityDto>();
            var usedCategories = new List<CategoryDto>();

            var newActivities = new List<ActivityDto>();
            var newCategories = new List<CategoryDto>();
            foreach (var entry in entries)
            {
                var trimmedEntry = regex.Replace(entry, " ").Trim();
                var splits = trimmedEntry.Split(":");
                var activity = trimmedEntry;
                var category = splits.Length > 1 ? splits[0] : "";

                var c = category.Length == 0 ? defaultCategory : categories.FirstOrDefault(x => x.Name == category);
                if (c is null)
                {
                    c = new CategoryDto(splits[0], 0, Color.White);
                    newCategories.Add(c);
                }

                var a = activities.FirstOrDefault(x => x.Name == activity);
                if (a is null)
                {
                    a = new ActivityDto(activity, c, false);
                    newActivities.Add(a);
                }
                usedActivities.Add(a);
                usedCategories.Add(c);
            }

            return (usedActivities, usedCategories, newActivities, newCategories);
        }
    }
}
