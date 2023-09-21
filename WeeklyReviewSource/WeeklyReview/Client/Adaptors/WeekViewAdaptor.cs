using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.HeatMap.Internal;
using Syncfusion.Blazor.Schedule.Internal;
using WeeklyReview.Client.Services;
using WeeklyReview.Client.ViewModels;

namespace WeeklyReview.Client.Adaptors
{
    public class WeekViewAdaptor : DataAdaptor
    {
        [Inject]
        public IClientWeeklyReviewService WeeklyReviewService { get; set; }
        public Guid UserGuid = new Guid("24fe9480-4e7a-4515-b96c-248171496591");

        List<ScheduleViewModel> EventData = new List<ScheduleViewModel>();

        public async override Task<object> ReadAsync(DataManagerRequest dataManagerRequest, string key = null)
        {
            EventData = new List<ScheduleViewModel>();
            var entries = (await WeeklyReviewService.Entry.GetAll(UserGuid)).ToList();
            foreach (var entry in entries)
            {
                var s = new ScheduleViewModel();
                s.Subject = entry.Activities.ConvertAll(x => x.Name).Aggregate((x, y) => x + " + " + y);
                s.StartTime = entry.StartTime;
                s.EndTime = entry.EndTime is null ? entry.StartTime.AddHours(12) : entry.EndTime.Value;
                var primaryCat = entry.Activities.ConvertAll(x => x.Category).MaxBy(x => x.Priority);
                s.CategoryId = primaryCat.Id;
                s.Color = primaryCat.Color;
                EventData.Add(s);
            }

            await Task.Delay(100); //To mimic asynchronous operation, we delayed this operation using Task.Delay
            return dataManagerRequest.RequiresCounts ? new DataResult() { Result = EventData, Count = EventData.Count() } : (object)EventData;
        }
    }
}
