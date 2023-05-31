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
using System.Drawing;

namespace WeeklyReview.Client.Pages
{
    public partial class WeekView
    {
        public DateTime CurrentDate = DateTime.Now;
        public List<ScheduleEntry> DataSource { get; set; } = new List<ScheduleEntry>();
        public List<CatagoryColor> Colors { get; set; } = new List<CatagoryColor>();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            for (int i = 0; i < 20; i++)
            {
                DataSource.Add(new ScheduleEntry("Entry " + i, i % 2 == 0 ? 1 : 2));
            }

            Colors.Add(new CatagoryColor(1, "rgb(100,10,100"));
            Colors.Add(new CatagoryColor(2, "rgb(10,100,200"));
        }
    }

    public class CatagoryColor
    {
        public int Id { get; set; }
        public string Color { get; set; }

        public CatagoryColor(int id, string color)
        {
            Id = id;
            Color = color;
        }
    }

    public class ScheduleEntry
    {
        static DateTime baseDate = new DateTime(2023, 05, 29);
        static int currentMaxId = 1;
        static int NextId => ++currentMaxId;

        public ScheduleEntry()
        {
        }

        public ScheduleEntry(string name, int colorId)
        {
            Id = NextId;
            Name = name;
            StartTime = baseDate.AddHours(Id * 2);
            EndTime = StartTime.AddHours(2);
            ColorId = colorId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ColorId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}