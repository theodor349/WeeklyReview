using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeeklyReview.Shared.Models
{
    public class EnterEntryModel
    {
        public List<string> Entries { get; set; } = new List<string>();
        public DateTime Date { get; set; }
    }
}
