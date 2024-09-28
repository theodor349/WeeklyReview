namespace Shared.Models.DTOs;

public class GetEntriesAroundDto
{
    public DateTime Date { get; set; }
    public int DaysAround { get; set; }

    public GetEntriesAroundDto(DateTime date, int daysAround)
    {
        Date = date;
        DaysAround = daysAround;
    }
}
