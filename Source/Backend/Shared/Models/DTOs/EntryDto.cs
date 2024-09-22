namespace shared.Models.DTOs;

public class EntryDto : IComparable<EntryDto>
{
    private static int _entryIndex = 1;
    private static int _nextIndex => _entryIndex++;

    public int Id { get; set; }
    public DateTime StarTime { get; set; }
    public DateTime EndTime { get; set; }
    public DateTime Entered { get; set; }
    public List<ActivityDto> Activities { get; set; } = new List<ActivityDto>();

    public EntryDto()
    {
        Id = _nextIndex;
    }

    public int CompareTo(EntryDto? other)
    {
        return StarTime.CompareTo(other.StarTime);
    }
}
