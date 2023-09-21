using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.HeatMap.Internal;
using WeeklyReview.Client.Services;
using WeeklyReview.Client.ViewModels;
using WeeklyReview.Database.Models;
using WeeklyReview.Shared.Services;

namespace WeeklyReview.Client.Pages
{
    public partial class WeekView
    {
        [Inject]
        public IClientWeeklyReviewService WeeklyReviewService { get; set; }
        public Guid UserGuid = new Guid("24fe9480-4e7a-4515-b96c-248171496591");

        public DateTime InputDate = DateTime.Now;
        public DateTime ViewDate = DateTime.Now;
        public List<ScheduleViewModel> DataSource { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public IEnumerable<ActivityModel> Activities = new List<ActivityModel>();
        public async Task<IEnumerable<ActivityModel>> GetActivities() => await WeeklyReviewService.Activity.GetAll(UserGuid);
        public string EnteredActivity { get; set; }

        protected override async void OnParametersSet()
        {
            if (DataSource is null) 
            {
                DataSource = new List<ScheduleViewModel>();
                Categories = new List<CategoryViewModel>();
                await GenerateViewModels();
                TimeUpdated();
            }
            base.OnParametersSet();
        }

        protected async override Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Activities = await GetActivities();
        }

        public void TimeUpdated()
        {
            int minutes = ViewDate.Minute;
            minutes /= 3;
            minutes *= 15;
            ViewDate.AddMinutes(minutes - ViewDate.Minute);
        }

        private async Task GenerateViewModels()
        {
            var categories = (await WeeklyReviewService.Category.GetAll(UserGuid)).ToList();
            foreach (var cat in categories)
            {
                Categories.Add(new CategoryViewModel(cat));
            }

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
                DataSource.Add(s);
            }
        }
    }
}