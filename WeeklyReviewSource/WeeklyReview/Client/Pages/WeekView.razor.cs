using Microsoft.AspNetCore.Components;
using System.Drawing;
using WeeklyReview.Client.Services;
using WeeklyReview.Client.ViewModels;
using WeeklyReview.Shared.Models;

namespace WeeklyReview.Client.Pages
{
    public partial class WeekView
    {
        [Inject]
        public WeeklyReviewService WRService { get; set; }

        public DateTime InputDate = DateTime.Now;
        public DateTime ViewDate = DateTime.Now;
        public List<ScheduleViewModel> DataSource { get; set; } = new List<ScheduleViewModel>();
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if(DataSource.Count == 0 ) 
            {
                GenerateViewModels();
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

        private void GenerateViewModels()
        {
            foreach (var cat in WRService.Categories)
            {
                Categories.Add(new CategoryViewModel(cat));
            }

            foreach (var entry in WRService.Entries)
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