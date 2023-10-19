namespace DataMigration.Models
{
    public class EntryModel
    {
        public List<string> Entries { get; set; } = new List<string>();
        public DateTime Date { get; set; }
    }
}
