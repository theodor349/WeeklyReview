using System.Drawing;

namespace WeeklyReview.Shared.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public Color Color { get; set; }

        public CategoryModel(int id, string name, int priority, Color color)
        {
            Id = id;
            Name = name;
            Priority = priority;
            Color = color;
        }
    }
}