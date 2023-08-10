using System.Drawing;

namespace WeeklyReview.Database.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public int Priority { get; set; }
        public Color Color { get; set; }

        private CategoryModel()
        {
            
        }

        public CategoryModel(string name, int priority, Color color)
        {
            Name = name;
            Priority = priority;
            Color = color;
            NormalizedName = name.ToLower();
        }
    }
}