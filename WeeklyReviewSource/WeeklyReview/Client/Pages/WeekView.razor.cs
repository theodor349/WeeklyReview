using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;
using System.Drawing;
using WeeklyReview.Client.Services;
using WeeklyReview.Client.ViewModels;
using WeeklyReview.Database.Models;
using WeeklyReview.Shared.Extensions;

namespace WeeklyReview.Client.Pages
{
    public partial class WeekView
    {
        [Inject]
        public IClientWeeklyReviewService WeeklyReviewService { get; set; }
        public Guid UserGuid = new Guid("24fe9480-4e7a-4515-b96c-248171496591");

        public DateTime InputDate = DateTime.Now;
        public DateTime ViewDate = DateTime.Now;
        private DateTime LastMidleOfTheMonth = DateTime.Now;
        private DateTime LastDateUpdate = DateTime.MinValue;
        public ObservableCollection<ScheduleViewModel> DataSource { get; set; } = new ObservableCollection<ScheduleViewModel>();

        protected async override Task OnInitializedAsync()
        {
            if (DataSource.Count() == 0)
            {
                await Update();
            }

            await this.ScheduleObj.ScrollToAsync("08:00");
            await base.OnInitializedAsync();
        }

        private async Task OnDateChanged(DateTime newDate)
        {
            ViewDate = newDate;
            await Update();
        }

        private async Task Update()
        {
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
            var middleOfTheMonth = new DateTime(ViewDate.Year, ViewDate.Month, 16);
            bool shouldUpdate = middleOfTheMonth != LastMidleOfTheMonth || (DateTime.Now - LastDateUpdate) > TimeSpan.FromMinutes(1);

            if (shouldUpdate)
            {
                LastDateUpdate = DateTime.Now;
                LastMidleOfTheMonth = middleOfTheMonth;

                var entries = (await WeeklyReviewService.Entry.GetAllAroundDate(UserGuid, middleOfTheMonth, 21)).ToList();
                DataSource.Clear();
                foreach (var entry in entries)
                {
                    AddScheduleEntry(entry);
                }
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
            var c = primaryCat.Color;
            s.RgbColorString = c.ToRgb();
            s.RgbR = c.R;
            s.RgbG = c.G;
            s.RgbB = c.B;
            DataSource.Add(s);
        }

        private Color RandomColor()
        {
            var r = new Random();
            return Color.FromArgb(r.Next(0, 255), r.Next(0, 255), r.Next(0, 255));
        }
    }
}