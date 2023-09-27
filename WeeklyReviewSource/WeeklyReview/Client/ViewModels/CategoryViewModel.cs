using WeeklyReview.Database.Models;
using WeeklyReview.Shared.Extensions;
using WeeklyReview.Shared.Models.DTOs;

namespace WeeklyReview.Client.ViewModels
{
    public class CategoryViewModel
    {
        private CategoryModel _category;

        public CategoryViewModel(CategoryModel category)
        {
            _category = category;
        }

        public int Id => _category.Id;
        public string Color => _category.Color.ToRgb();
    }
}
