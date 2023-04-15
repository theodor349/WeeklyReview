using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UiLibrary.CalenderVeiw
{
    public class Appointment
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Text { get; set; }
        public Color Color { get; set; }

        public Appointment(DateTime start, DateTime end, string text, Color color)
        {
            Start = start;
            End = end;
            Text = text;
            Color = color;
        }
    }

    public class Entry : IComparable<Entry>
    {
        public DateTime Date { get; set; }
        public string Activity { get; set; }
        public Color Color { get; set; }

        public Entry(DateTime date, string activity, Color color)
        {
            Date = date;
            Activity = activity;
            Color = color;
        }

        public int CompareTo(Entry? other)
        {
            if(other == null) return -1;
            return -other.Date.CompareTo(Date);
        }
    }
}
