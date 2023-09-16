using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Shared.Models;
using WeeklyReview.Shared.Models.DTOs;

namespace WeeklyReview.Shared.Services
{
    internal class EntryAdderService : IEntryAdderService
    {
        // TODO: If an entry changes endtime, then all entries including deleted ones should be updated.
        private readonly IEntryParserService _entryParser;
        private readonly IDataService _dataService;

        public EntryAdderService(IEntryParserService entryParser, IDataService dataService)
        {
            _entryParser = entryParser;
            _dataService = dataService;
        }

        public async Task AddEntry(DateTime date, List<string> activities)
        {
            var Activities = await _dataService.GetActivities();
            var Categories = await _dataService.GetCategories();
            var defaultActivity = await _dataService.GetDefaultCategory();

            var res = _entryParser.ParseEntries(activities, Activities, Categories, defaultActivity);
            bool isEmpty = res.usedActivities.Count() == 0;

            if (isEmpty)
                await HandleEmpty(date);
            else
                await HandleNewEntry(date, res);
        }

        private async Task HandleNewEntry(DateTime date, (List<ActivityDto> usedActivities, List<CategoryDto> usedCategories, List<ActivityDto> newActivities, List<CategoryDto> newCategories) res)
        {
            await _dataService.AddActivities(res.newActivities);
            await _dataService.AddCategories(res.newCategories);

            var endTime = date.AddDays(1);
            var otherEntry = await _dataService.GetBeforeOrEqualEntry(date);
            if (otherEntry is not null)
            {
                endTime = otherEntry.EndTime;
                otherEntry.EndTime = date;
                if (otherEntry.StarTime == date)
                    await _dataService.RemoveEntry(otherEntry);
            }

            var e = new EntryDto();
            e.StarTime = date;
            e.EndTime = endTime;
            e.Entered = DateTime.Now;
            e.Activities.AddRange(res.usedActivities);
            await _dataService.AddEntry(e);
        }

        private async Task HandleEmpty(DateTime date)
        {
            var overriddenEntry = await _dataService.GetEqualEntry(date);
            if (overriddenEntry is null)
                return;

            await _dataService.RemoveEntry(overriddenEntry);

            var beforeEntry = await _dataService.GetBeforeEntry(date);
            var afterEntry = await _dataService.GetAfterEntry(date);
            if (beforeEntry is null || afterEntry is null)
                return;

            beforeEntry.EndTime = afterEntry.StarTime;
        }
    }
}
