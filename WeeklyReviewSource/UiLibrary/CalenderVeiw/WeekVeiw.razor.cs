using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using System.Drawing;

namespace UiLibrary.CalenderVeiw
{
    public partial class WeekVeiw : ComponentBase
    {
        RadzenScheduler<Appointment> scheduler;
        Dictionary<DateTime, string> events = new Dictionary<DateTime, string>();
        IList<Appointment> appointments = new List<Appointment>();
        List<Entry> entries = new List<Entry>();
        protected override void OnInitialized()
        {
            GenerateEntries();
            GenerateAppointments();
            base.OnInitialized();
        }

        void GenerateEntries()
        {
            var tempDate = DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek + 1);
            var monday = new DateTime(tempDate.Year, tempDate.Month, tempDate.Day);

            // Monday
            entries.Add(new Entry(monday.AddDays(0).AddHours(7).AddMinutes(30), "Breakfast", Color.Yellow));
            entries.Add(new Entry(monday.AddDays(0).AddHours(8).AddMinutes(0), "Bike", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(0).AddHours(8).AddMinutes(15), "School", Color.Pink));
            entries.Add(new Entry(monday.AddDays(0).AddHours(12).AddMinutes(45), "Bike", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(0).AddHours(13).AddMinutes(0), "Work: ITS", Color.LightGray));
            entries.Add(new Entry(monday.AddDays(0).AddHours(14).AddMinutes(45), "Sleep", Color.Purple));
            entries.Add(new Entry(monday.AddDays(0).AddHours(15).AddMinutes(45), "Make Food", Color.Green));
            entries.Add(new Entry(monday.AddDays(0).AddHours(16).AddMinutes(15), "Dinner + Youtube", Color.Yellow));
            entries.Add(new Entry(monday.AddDays(0).AddHours(17).AddMinutes(0), "Project: Weekly Review", Color.Green));
            entries.Add(new Entry(monday.AddDays(0).AddHours(18).AddMinutes(45), "Bike", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(0).AddHours(19).AddMinutes(30), "Jujitsu", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(0).AddHours(21).AddMinutes(30), "Bike + Grocery Shopping", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(0).AddHours(22).AddMinutes(0), "Movie", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(0).AddHours(23), "Sleep", Color.Purple));

            // Tuesday
            entries.Add(new Entry(monday.AddDays(1).AddHours(8).AddMinutes(15), "Breakfast", Color.Yellow));
            entries.Add(new Entry(monday.AddDays(1).AddHours(8).AddMinutes(45), "Bike", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(1).AddHours(9).AddMinutes(0), "School", Color.Pink));
            entries.Add(new Entry(monday.AddDays(1).AddHours(12).AddMinutes(45), "Bike", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(1).AddHours(13).AddMinutes(0), "School: Project", Color.Green));
            entries.Add(new Entry(monday.AddDays(1).AddHours(14).AddMinutes(45), "Sleep", Color.Purple));
            entries.Add(new Entry(monday.AddDays(1).AddHours(15).AddMinutes(45), "Bath", Color.Yellow));
            entries.Add(new Entry(monday.AddDays(1).AddHours(16).AddMinutes(15), "Bus", Color.LightBlue));
            entries.Add(new Entry(monday.AddDays(1).AddHours(17).AddMinutes(0), "Bachata", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(1).AddHours(18).AddMinutes(30), "Bus", Color.LightBlue));
            entries.Add(new Entry(monday.AddDays(1).AddHours(19).AddMinutes(0), "Event: Communal Dining", Color.LightBlue));
            entries.Add(new Entry(monday.AddDays(1).AddHours(21).AddMinutes(0), "Social: College Dorm", Color.LightBlue));
            entries.Add(new Entry(monday.AddDays(1).AddHours(23), "Sleep", Color.Purple));

            // Wedensday
            entries.Add(new Entry(monday.AddDays(2).AddHours(7).AddMinutes(30), "Breakfast", Color.Yellow));
            entries.Add(new Entry(monday.AddDays(2).AddHours(8).AddMinutes(0), "Bike", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(2).AddHours(8).AddMinutes(15), "School", Color.Pink));
            entries.Add(new Entry(monday.AddDays(2).AddHours(12).AddMinutes(45), "Bike", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(2).AddHours(13).AddMinutes(0), "Project: Weekly Review", Color.Green));
            entries.Add(new Entry(monday.AddDays(2).AddHours(14).AddMinutes(45), "Sleep", Color.Purple));
            entries.Add(new Entry(monday.AddDays(2).AddHours(15).AddMinutes(45), "Make Food", Color.Green));
            entries.Add(new Entry(monday.AddDays(2).AddHours(16).AddMinutes(15), "Dinner + Youtube", Color.Yellow));
            entries.Add(new Entry(monday.AddDays(2).AddHours(17).AddMinutes(0), "Project: Weekly Review", Color.Green));
            entries.Add(new Entry(monday.AddDays(2).AddHours(18).AddMinutes(45), "Bike", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(2).AddHours(19).AddMinutes(30), "Jujitsu", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(2).AddHours(21).AddMinutes(30), "Bike + Grocery Shopping", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(2).AddHours(22).AddMinutes(0), "Movie", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(2).AddHours(23), "Sleep", Color.Purple));

            // Thursday
            entries.Add(new Entry(monday.AddDays(3).AddHours(7).AddMinutes(45), "Bath", Color.Yellow));
            entries.Add(new Entry(monday.AddDays(3).AddHours(8).AddMinutes(15), "Breakfast", Color.Yellow));
            entries.Add(new Entry(monday.AddDays(3).AddHours(8).AddMinutes(45), "Bike", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(3).AddHours(9).AddMinutes(0), "School", Color.Pink));
            entries.Add(new Entry(monday.AddDays(3).AddHours(14).AddMinutes(30), "Bike", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(3).AddHours(14).AddMinutes(45), "Sleep", Color.Purple));
            entries.Add(new Entry(monday.AddDays(3).AddHours(15).AddMinutes(45), "Make Food", Color.Green));
            entries.Add(new Entry(monday.AddDays(3).AddHours(16).AddMinutes(15), "Dinner + Youtube", Color.Yellow));
            entries.Add(new Entry(monday.AddDays(3).AddHours(17).AddMinutes(0), "Project: Weekly Review", Color.Green));
            entries.Add(new Entry(monday.AddDays(3).AddHours(22).AddMinutes(0), "Movie", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(3).AddHours(23), "Sleep", Color.Purple));

            // Friday
            entries.Add(new Entry(monday.AddDays(4).AddHours(7).AddMinutes(30), "Breakfast", Color.Yellow));
            entries.Add(new Entry(monday.AddDays(4).AddHours(8).AddMinutes(0), "Bike", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(4).AddHours(8).AddMinutes(15), "Work", Color.Pink));
            entries.Add(new Entry(monday.AddDays(4).AddHours(14).AddMinutes(30), "Bike", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(4).AddHours(14).AddMinutes(45), "Sleep", Color.Purple));
            entries.Add(new Entry(monday.AddDays(4).AddHours(15).AddMinutes(45), "Make Food", Color.Green));
            entries.Add(new Entry(monday.AddDays(4).AddHours(16).AddMinutes(15), "Dinner + Youtube", Color.Yellow));
            entries.Add(new Entry(monday.AddDays(4).AddHours(16).AddMinutes(45), "Bath", Color.Yellow));
            entries.Add(new Entry(monday.AddDays(4).AddHours(17).AddMinutes(15), "Bus", Color.LightBlue));
            entries.Add(new Entry(monday.AddDays(4).AddHours(18).AddMinutes(0), "Party", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(5).AddHours(4).AddMinutes(30), "Bus", Color.LightBlue));
            entries.Add(new Entry(monday.AddDays(5).AddHours(5), "Sleep", Color.Purple));

            // Saturday
            entries.Add(new Entry(monday.AddDays(5).AddHours(12).AddMinutes(0), "Breakfast + Youtube", Color.Yellow));
            entries.Add(new Entry(monday.AddDays(5).AddHours(13), "Sleep", Color.Purple));
            entries.Add(new Entry(monday.AddDays(5).AddHours(15).AddMinutes(45), "Make Food", Color.Green));
            entries.Add(new Entry(monday.AddDays(5).AddHours(16).AddMinutes(15), "Dinner + Youtube", Color.Yellow));
            entries.Add(new Entry(monday.AddDays(5).AddHours(17).AddMinutes(0), "Project: Weekly Review", Color.Green));
            entries.Add(new Entry(monday.AddDays(5).AddHours(22).AddMinutes(0), "Movie", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(5).AddHours(23), "Sleep", Color.Purple));

            // Sunday
            entries.Add(new Entry(monday.AddDays(6).AddHours(9).AddMinutes(0), "Breakfast + Youtube", Color.Yellow));
            entries.Add(new Entry(monday.AddDays(6).AddHours(10).AddMinutes(0), "Project: Weekly Review", Color.Green));
            entries.Add(new Entry(monday.AddDays(6).AddHours(12).AddMinutes(0), "Lunch + Youtube", Color.Yellow));
            entries.Add(new Entry(monday.AddDays(6).AddHours(14).AddMinutes(0), "Project: Weekly Review", Color.Green));
            entries.Add(new Entry(monday.AddDays(6).AddHours(15).AddMinutes(15), "Bus", Color.LightBlue));
            entries.Add(new Entry(monday.AddDays(6).AddHours(16).AddMinutes(0), "Salsa", Color.ForestGreen));
            entries.Add(new Entry(monday.AddDays(6).AddHours(18).AddMinutes(15), "Bus", Color.LightBlue));
            entries.Add(new Entry(monday.AddDays(6).AddHours(18).AddMinutes(45), "Dinner", Color.Yellow));
            entries.Add(new Entry(monday.AddDays(6).AddHours(19).AddMinutes(0), "EU4 Weekly", Color.Orange));
            entries.Add(new Entry(monday.AddDays(6).AddHours(23), "Sleep", Color.Purple));

            entries.Sort();
        }

        void GenerateAppointments()
        {
            for (int i = 0; i < entries.Count(); i++)
            {
                var current = entries[i];
                DateTime end;
                if (i < entries.Count() - 1)
                    end = entries[i + 1].Date;
                else
                    end = current.Date.AddDays(1);
                appointments.Add(new Appointment(current.Date, end, current.Activity, current.Color));
            }
        }

        void OnSlotRender(SchedulerSlotRenderEventArgs args)
        {
            // Highlight today in month view
            if (args.View.Text == "Month" && args.Start.Date == DateTime.Today)
            {
                args.Attributes["style"] = "background: rgba(255,220,40,.2);";
            }

            // Highlight working hours (8-22)
            if ((args.View.Text == "Week" || args.View.Text == "Day") && args.Start.Hour >= 8 && args.Start.Hour <= 22)
            {
                args.Attributes["style"] = "background: rgba(255,220,40,.2);";
            }
        }
    }
}