using Microsoft.AspNetCore.Components;
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

        public IEnumerable<ActivityModel> Activities = new List<ActivityModel>();
        public IEnumerable<string> Socials = new List<string>();
        public async Task<IEnumerable<ActivityModel>> GetActivities()
            => await WeeklyReviewService.Activity.GetAll(UserGuid);

        public async Task<IEnumerable<string>> GetSocials()
            => (await WeeklyReviewService.Activity.GetAll(UserGuid)).ToList().ConvertAll(x => x.Name);

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
         
            Activities = await GetActivities();
            Socials = await GetSocials();
        }

        public void SubmitEntry()
        {
            var submittedActivities = new List<string>();
            submittedActivities.AddRange(InputActivities.Where(x => !string.IsNullOrWhiteSpace(x)));
            submittedActivities.AddRange(InputSocials.Where(x => !string.IsNullOrWhiteSpace(x)).ToList().ConvertAll(x => IsDiscord ? "Discord: " + x : "Social: " + x));

            WeeklyReviewService.Entry.Create(new EnterEntryModel() { Date = ViewDate, Entries = submittedActivities }, UserGuid);

            if(OnAfterEntryAdded != null) 
                OnAfterEntryAdded.Invoke();
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