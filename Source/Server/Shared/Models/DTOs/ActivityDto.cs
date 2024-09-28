namespace Shared.Models.DTOs;

public class ActivityDto
{
    private static int _ActivityIndex = 1;
    private static int _nextIndex => _ActivityIndex++;

    public int Id { get; set; }
    public string Name { get; set; }
    public CategoryDto Category { get; set; }

    public ActivityDto()
    {

    }

    public ActivityDto(string name, CategoryDto category, bool includeCategory = true)
    {
        Id = _nextIndex;
        Category = category;

        if (includeCategory)
            Name = category.Name + ": " + name;
        else
            Name = name;
    }
}
