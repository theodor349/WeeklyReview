using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeeklyReview.Shared.Services
{
    public interface IWeeklyReviewService
    {
        public IActivityChangeService ActivityChange { get; set; }
        public IActivityService Activity { get; set; }
        public ICategoryService Category { get; set; }
        public IEntryService Entry { get; set; }
    }

    public interface IActivityService
    {

    }

    public interface ICategoryService
    {

    }

    public interface IEntryService
    {

    }
}
