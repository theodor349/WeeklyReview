using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMigration.Models
{
    public class RecordTimeModel : IComparable<RecordTimeModel>
    {
        public DateTime Date { get; set; }
        public int Minutes { get; set; }

        public int CompareTo(RecordTimeModel other)
        {
            return DateTime.Compare(Date, other.Date);
        }
    }
}
