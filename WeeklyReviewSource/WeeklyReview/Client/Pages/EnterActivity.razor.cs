using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Microsoft.JSInterop;
using WeeklyReview.Client;
using WeeklyReview.Client.Shared;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Calendars;
using Syncfusion.Blazor.Schedule;
using Syncfusion.Blazor.Cards;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.SplitButtons;
using Syncfusion.Blazor.DropDowns;
using WeeklyReview.Client.ViewModels;
using WeeklyReview.Shared.Models;
using WeeklyReview.Client.Services;

namespace WeeklyReview.Client.Pages
{
    public partial class EnterActivity
    {
        [Inject]
        public WeeklyReviewService WRService { get; set; }

        public DateTime ViewDate = DateTime.Now;
        public List<ScheduleViewModel> DataSource { get; set; } = new List<ScheduleViewModel>();
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public List<Activity> Activities => WRService.Activities;
        public List<Activity> Socials => WRService.Activities;
        public List<string> InputActivities { get; set; } = new List<string>();
        public List<string> InputSocials { get; set; } = new List<string>();

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (DataSource.Count == 0)
            {
                GenerateViewModels();
                TimeUpdated();
            }

            AddInputActivity();
            AddInputSocial();
        }

        private void AddInputActivity()
        {
            InputActivities.Add("");
        }

        private void RemoveInputActivity()
        {
            InputActivities.RemoveAt(InputActivities.Count() - 1);
            if (InputActivities.Count == 0)
                AddInputActivity();
        }

        private void AddInputSocial()
        {
            InputSocials.Add("");
        }

        private void RemoveInputSocial()
        {
            InputSocials.RemoveAt(InputSocials.Count() - 1);
            if (InputSocials.Count == 0)
                AddInputSocial();
        }

        public void TimeUpdated()
        {
            int minutes = ViewDate.Minute;
            minutes /= 15;
            minutes *= 15;
            ViewDate = ViewDate.AddMinutes(minutes - ViewDate.Minute);
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