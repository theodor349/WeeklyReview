using WeeklyReview.Shared.Extensions;
using WeeklyReview.Shared.Models.DTOs;

namespace WeeklyReview.Client.ViewModels
{
    public class CategoryViewModel
    {
        private CategoryDto _category;

        public CategoryViewModel(CategoryDto category)
        {
            _category = category;
        }

        public int Id => _category.Id;
        public string Color => _category.Color.ToRgb();
    }
}
