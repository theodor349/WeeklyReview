using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Collections.ObjectModel;
using System.Drawing;
using WeeklyReview.Client.ViewModels;
using WeeklyReview.Database.Models;

namespace WeeklyReview.Client.Pages
{
    public partial class ManageCategories
    {
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>()
        {
            new CategoryViewModel(new CategoryModel("Movies", 100, Color.Orange, Guid.NewGuid())),
            new CategoryViewModel(new CategoryModel("Sports", 1000, Color.DarkGreen, Guid.NewGuid())),
            new CategoryViewModel(new CategoryModel("Bath", 100, Color.Yellow, Guid.NewGuid())),
        };

    }
}