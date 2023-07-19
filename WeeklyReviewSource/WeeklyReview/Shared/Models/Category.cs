using System.Drawing;

namespace WeeklyReview.Shared.Models
{
    public class Category
    {
        private static int _CategoryIndex = 1;
        private static int _nextIndex => _CategoryIndex++;

        public Category()
        {
            
        }

        public Category(string name, int priority, Color color)
        {
            Id = _nextIndex;
            Name = name;
            Priority = priority;
            Color = color;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Priority { get; set; }
        public Color Color { get; set; }
    }

}
