using Microsoft.AspNetCore.Components;
using System.Text.RegularExpressions;
using WeeklyReview.Database.Models;
using WeeklyReview.Shared.Models;
using WeeklyReview.Shared.Services;

namespace WeeklyReview.Client.Pages
{
    public partial class EnterActivity
    {
        [Inject]
        public IWeeklyReviewService WeeklyReviewService { get; set; }
        public Guid UserGuid = new Guid("24fe9480-4e7a-4515-b96c-248171496591");

        public bool IsDiscord { get; set; }
        public List<string> InputActivities { get; set; } = new List<string>();
        public List<string> InputSocials { get; set; } = new List<string>();

        public List<ActivityModel> Activities = new List<ActivityModel>();
        public List<string> Socials = new List<string>();
        public async Task<List<ActivityModel>> GetActivities()
            => (await WeeklyReviewService.Activity.GetAll(UserGuid)).ToList();

        public async Task<List<string>> GetSocials()
            => (await WeeklyReviewService.Activity.GetAll(UserGuid)).ToList().ConvertAll(x => x.Name).ToList();

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            TimeUpdated();

            if (ResetOnInit)
            {
                for (int i = 0; i < InputActivities.Count; i++)
                {
                    RemoveInputActivity();
                }
                for (int i = 0; i < InputSocials.Count; i++)
                {
                    RemoveInputSocial();
                }
            }

            if (InputActivities.Count() == 0)
            {
                AddInputActivity();
                AddInputSocial();
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();

            await GetData();
        }

        private async Task GetData()
        {
            Activities = await GetActivities();
            Socials = await GetSocials();
        }

        public async Task SubmitEntry()
        {
            var submittedActivities = new List<string>();
            submittedActivities.AddRange(InputActivities.Where(x => !string.IsNullOrWhiteSpace(x)));
            submittedActivities.AddRange(InputSocials.Where(x => !string.IsNullOrWhiteSpace(x)).ToList().ConvertAll(x => IsDiscord ? "Discord: " + x : "Social: " + x));
            TrimActivities(submittedActivities);
            foreach (var act in submittedActivities)
            {
                if(!Activities.Any(x => x.Name == act))
                {
                    if (!await ConfirmNewEntry(act))
                        return;
                }
            }

            var entry = await WeeklyReviewService.Entry.Create(new EnterEntryModel() { Date = ViewDate, Entries = submittedActivities }, UserGuid);
            if (entry is not null)
                AddNewActivities(entry);

            await ShowSuccessToast();
            if (OnAfterEntryAdded != null) 
                OnAfterEntryAdded.Invoke(entry);
        }

        private void TrimActivities(List<string> submittedActivities)
        {
            for (int i = 0; i < submittedActivities.Count; i++)
            {
                submittedActivities[i] = TrimActivity(submittedActivities[i]);
            }
        }

        private string TrimActivity(string entry)
        {
            var splits = entry.Split(':');
            string? cat = null;
            string? act;
            if (splits.Count() > 2)
            {
                throw new ArgumentException("An entry cannot contain multiple ':'");
            }
            else if (splits.Count() == 2)
            {
                cat = splits[0];
                act = splits[1];
            }
            else
            {
                act = splits[0];
            }

            return TrimSentance(cat)  + ": " + TrimSentance(act);
        }

        private string? TrimSentance(string? sentance)
        {
            if (sentance is null)
                return null;

            var regX = new Regex("\\s+");
            var res = regX.Replace(sentance, " ").Trim();
            if (res.Count() > 0)
                return res;
            else
                return null;
        }

        private void AddNewActivities(EntryModel? entry)
        {
            var newActs = new List<ActivityModel>();
            foreach (var a in entry.Activities)
            {
                if (Activities.Any(x => x.Id == a.Id))
                    continue;
                newActs.Add(a);
            }
            Activities.AddRange(newActs);
        }

        private async Task<bool> ConfirmNewEntry(string act)
        {
            bool isConfirm = await DialogService.ConfirmAsync($"You have never entered ´{act}´, are you sure?", "Add new Activity");
            string confirmMessage = isConfirm ? "confirmed" : "canceled";
            return isConfirm;
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