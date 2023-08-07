using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeeklyReview.Database.Models
{
    public class ActivityModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NormalizedName { get; set; }
        public bool Deleted { get; set; }
        public CategoryModel? Category { get; set; }

        private ActivityModel()
        {
            
        }

        public ActivityModel(int id, string name, bool deleted, CategoryModel? category)
        {
            Id = id;
            Name = name;
            Deleted = deleted;
            Category = category;
            NormalizedName = name.ToLower();
        }
    }
}
