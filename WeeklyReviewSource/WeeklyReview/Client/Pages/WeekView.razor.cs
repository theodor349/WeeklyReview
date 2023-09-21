using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.HeatMap.Internal;
using System.Collections.ObjectModel;
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
        public ObservableCollection<ScheduleViewModel> DataSource { get; set; } = new ObservableCollection<ScheduleViewModel>();
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();

        protected async override Task OnInitializedAsync()
        {
            if (DataSource.Count() == 0)
            {
                await Update();
            }

            await base.OnInitializedAsync();
        }

        private async Task Update()
        {
            DataSource.Clear();
            Categories.Clear();
            await GenerateViewModels();
            TimeUpdated();
        }

        public void TimeUpdated()
        {
            int minutes = ViewDate.Minute;
            minutes /= 3;
            minutes *= 15;
            ViewDate.AddMinutes(minutes - ViewDate.Minute);
        }

        public async void OnEntryAdded(EntryModel? entry)
        {
            await ScheduleObj.CloseQuickInfoPopupAsync();
            await Update();
            StateHasChanged();
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
                AddScheduleEntry(entry);
            }
        }

        private void AddScheduleEntry(EntryModel? entry)
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