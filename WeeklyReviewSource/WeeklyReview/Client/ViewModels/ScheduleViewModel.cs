using Syncfusion.PdfExport;
using System.Drawing;

namespace WeeklyReview.Client.ViewModels
{
    public class ScheduleViewModel
    {
        public string Subject { get; set; }
        public int CategoryId { get; set; }
        public Color Color { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public TimeSpan Duration => EndTime - StartTime;
    }
}
