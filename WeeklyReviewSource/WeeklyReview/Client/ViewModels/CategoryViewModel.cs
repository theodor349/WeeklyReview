using WeeklyReview.Shared.Extensions;
using WeeklyReview.Shared.Models;

namespace WeeklyReview.Client.ViewModels
{
    public class CategoryViewModel
    {
        private Category _category;

        public CategoryViewModel(Category category)
        {
            _category = category;
        }

        public int Id => _category.Id;
        public string Color => _category.Color.ToRgb();
    }
}
