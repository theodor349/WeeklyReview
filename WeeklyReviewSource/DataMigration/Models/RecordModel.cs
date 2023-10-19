namespace DataMigration.Models
{
    public class RecordModel : IComparable<RecordModel>
    {
        public int Id { get; set; }
        public int ActivityId { get; set; }
        public DateTime RecordDate { get; set; }

        public int CompareTo(RecordModel other)
        {
            return DateTime.Compare(RecordDate, other.RecordDate);
        }
    }
}
