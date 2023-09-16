using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeeklyReview.Database.Models;
using WeeklyReview.Database.Persitance;

namespace WeeklyReview.Shared.Services
{
    internal class CategoryService : ICategoryService
    {
        private readonly WeeklyReviewDbContext _db;

        public CategoryService(WeeklyReviewDbContext db)
        {
            _db = db;
        }

        public IEnumerable<CategoryModel> GetAll(Guid userGuid)
        {
            return _db.Category.Where(x => x.UserGuid == userGuid);
        }

        public CategoryModel? Get(int key, Guid userGuid)
        {
            return _db.Category.SingleOrDefault(x => x.Id == key && x.UserGuid == userGuid);
        }

        public CategoryModel Delete(int key, Guid userGuid)
        {
            var model = _db.Category.SingleOrDefault(x => x.Id == key && x.UserGuid == userGuid);
            if (model is null)
                throw new KeyNotFoundException($"Model not found with id {key}");

            var activitiesReferencesActivity = _db.Activity.Include(x => x.Category).Any(x => x.Category == model && x.Deleted == false);
            if (activitiesReferencesActivity)
                throw new InvalidOperationException($"It is not possible to delete a category which is still referenced by activities");

            model.Deleted = true;
            _db.SaveChanges();
            return model;
        }

        public CategoryModel ChangeColor(int key, Color color, Guid userGuid)
        {
            var model = _db.Category.SingleOrDefault(x => x.Id == key && x.UserGuid == userGuid);
            if (model is null)
                throw new KeyNotFoundException($"Model not found with id {key}");
            model.Color = color;
            _db.SaveChanges();
            return model;
        }
    }
}
