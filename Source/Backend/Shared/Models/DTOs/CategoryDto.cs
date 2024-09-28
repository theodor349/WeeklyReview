using System.Drawing;

namespace Shared.Models.DTOs;

public class CategoryDto
{
    private static int _CategoryIndex = 1;
    private static int _nextIndex => _CategoryIndex++;

    public int Id { get; set; }
    public string Name { get; set; }
    public int Priority { get; set; }
    public Color Color { get; set; }

    public CategoryDto()
    {

    }

    public CategoryDto(string name, int priority, Color color)
    {
        Id = _nextIndex;
        Name = name;
        Priority = priority;
        Color = color;
    }
}
