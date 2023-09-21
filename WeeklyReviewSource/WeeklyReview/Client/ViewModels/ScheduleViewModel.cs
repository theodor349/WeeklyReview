using Syncfusion.PdfExport;
using System.ComponentModel;
using System.Drawing;
using WeeklyReview.Shared.Extensions;

namespace WeeklyReview.Client.ViewModels
{
    public class ScheduleViewModel : INotifyPropertyChanged
    {
        public string Subject { get; set; }
        public int CategoryId { get; set; }
        public Color Color { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public TimeSpan Duration => EndTime - StartTime;

        public string RgbColor => Color.ToRgb();
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
