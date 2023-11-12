using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;
using WeeklyReview.Client.ViewModels;
using WeeklyReview.Database.Models;

namespace WeeklyReview.Client.Pages
{
    public partial class ManageCategories
    {
        public ObservableCollection<CategoryModel> MyProperty { get; set; }

    }
}