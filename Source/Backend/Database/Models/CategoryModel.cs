using database.Converters;
using System.Drawing;
using System.Text.Json.Serialization;

public class CategoryModel : ICloneable
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string NormalizedName { get; set; }
    public int Priority { get; set; }
    [JsonConverter(typeof(JsonColorConverter))]
    public Color Color { get; set; }
    public bool Deleted { get; set; }
    public Guid UserGuid { get; set; }

    public CategoryModel()
    {

    }

    public CategoryModel(string name, int priority, Color color, Guid userGuid)
    {
        Name = name;
        Priority = priority;
        Color = color;
        NormalizedName = name.ToLower();
        UserGuid = userGuid;
    }

    public CategoryModel(string name, int priority, Color color, bool deleted, Guid userGuid)
    {
        Name = name;
        Priority = priority;
        Color = color;
        NormalizedName = name.ToLower();
        Deleted = deleted;
        UserGuid = userGuid;
    }

    public object Clone()
    {
        return new CategoryModel()
        {
            Id = Id,
            Name = Name,
            NormalizedName = NormalizedName,
            Priority = Priority,
            Color = Color,
            Deleted = Deleted,
            UserGuid = UserGuid
        };
    }
}