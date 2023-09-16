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
using WeeklyReview.Client.Services;
using WeeklyReview.Shared.Models.DTOs;
using WeeklyReview.Shared.Services;
using WeeklyReview.Database.Models;
using WeeklyReview.Shared.Models;

namespace WeeklyReview.Client.Pages
{
    public partial class EnterActivity
    {
        [Inject]
        public IWeeklyReviewService WeeklyReviewService { get; set; }
        public Guid UserGuid = new Guid("24fe9480-4e7a-4515-b96c-248171496591");

        public DateTime ViewDate = DateTime.Now;
        public bool IsDiscord { get; set; }
        public List<string> InputActivities { get; set; } = new List<string>();
        public List<string> InputSocials { get; set; } = new List<string>();

        public IEnumerable<ActivityModel> Activities
        {
            get
            {
                return WeeklyReviewService.Activity.GetAll(UserGuid);
            }
        }

        public IEnumerable<string> Socials
        {
            get
            {
                // TODO: Add socials to interface
                return WeeklyReviewService.Activity.GetAll(UserGuid).ToList().ConvertAll(x => x.Name);
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            TimeUpdated();

            AddInputActivity();
            AddInputSocial();
        }

        public void SubmitEntry()
        {
            var submittedActivities = new List<string>();
            submittedActivities.AddRange(InputActivities.Where(x => !string.IsNullOrWhiteSpace(x)));
            submittedActivities.AddRange(InputSocials.Where(x => !string.IsNullOrWhiteSpace(x)).ToList().ConvertAll(x => IsDiscord ? "Discord: " + x : "Social: " + x));

            WeeklyReviewService.Entry.Create(new EnterEntryModel() { Date = ViewDate, Entries = submittedActivities }, UserGuid);

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