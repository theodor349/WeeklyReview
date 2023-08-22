using Microsoft.AspNetCore.Components;
using System.Drawing;
using WeeklyReview.Client.Services;
using WeeklyReview.Client.ViewModels;
using WeeklyReview.Shared.Models.DTOs;
using WeeklyReview.Shared.Services;

namespace WeeklyReview.Client.Pages
{
    public partial class WeekView
    {
        [Inject]
        public IDataService _dataService { get; set; }

        public DateTime InputDate = DateTime.Now;
        public DateTime ViewDate = DateTime.Now;
        public List<ScheduleViewModel> DataSource { get; set; } = new List<ScheduleViewModel>();
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public List<ActivityDto> Activities
        {
            get
            {
                var task = _dataService.GetActivities();
                task.Wait();
                return task.Result.ToList();
            }
        }
        public string EnteredActivity { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
         
            if (DataSource.Count == 0 ) 
            {
                await GenerateViewModels();
                TimeUpdated();
            }
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
            foreach (var cat in await _dataService.GetCategories())
            {
                Categories.Add(new CategoryViewModel(cat));
            }

            foreach (var entry in await _dataService.GetEntries())
            {
                var s = new ScheduleViewModel();
                s.Subject = entry.Activities.ConvertAll(x => x.Name).Aggregate((x, y) => x + " + " + y);
                s.StartTime = entry.StarTime;
                s.EndTime = entry.EndTime;
                s.CategoryId = entry.Activities.ConvertAll(x => x.Category).MaxBy(x => x.Priority).Id;
                DataSource.Add(s);
            }
        }
    }
}