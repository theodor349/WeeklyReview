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
using WeeklyReview.Shared.Services;

namespace WeeklyReview.Client.Pages
{
    public partial class EnterActivity
    {
        [Inject]
        public IDataService DataService { get; set; }
        [Inject]
        public IEntryAdderService EntryAdderService { get; set; }

        public DateTime ViewDate = DateTime.Now;
        public bool IsDiscord { get; set; }
        public List<string> InputActivities { get; set; } = new List<string>();
        public List<string> InputSocials { get; set; } = new List<string>();

        public List<Activity> Activities
        {
            get
            {
                var task = DataService.GetActivities();
                task.Wait();
                return task.Result.ToList();
            }
        }

        public List<string> Socials
        {
            get
            {
                var task = DataService.GetSocials();
                task.Wait();
                return task.Result.ToList();
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            TimeUpdated();

            AddInputActivity();
            AddInputSocial();
        }

        public async Task SubmitEntry()
        {
            var submittedActivities = new List<string>();
            submittedActivities.AddRange(InputActivities.Where(x => !string.IsNullOrWhiteSpace(x)));
            submittedActivities.AddRange(InputSocials.Where(x => !string.IsNullOrWhiteSpace(x)).ToList().ConvertAll(x => IsDiscord ? "Discord: " + x : "Social: " + x));

            await EntryAdderService.AddEntry(ViewDate, submittedActivities);

            for (int i = 0; i < InputActivities.Count(); i++)
                RemoveInputActivity();
            for (int i = 0; i < InputSocials.Count(); i++)
                RemoveInputSocial();
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
            ViewDate = new DateTime(ViewDate.Year, ViewDate.Month, ViewDate.Day, ViewDate.Hour, ViewDate.Minute, 0);
            int minutes = ViewDate.Minute;
            minutes /= 15;
            minutes *= 15;
            ViewDate = ViewDate.AddMinutes(minutes - ViewDate.Minute);
        }
    }
}