using System.Drawing;

namespace WeeklyReview.Shared.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public Color Color { get; set; }
    }

}
