using database.Persitance;
using Microsoft.EntityFrameworkCore;
using System.Drawing;

namespace Shared.Services;

internal class CategoryService : ICategoryService
{
    private readonly WeeklyReviewDbContext _db;

    public CategoryService(WeeklyReviewDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<CategoryModel>> GetAll(Guid userGuid)
    {
        return await _db.Category.Where(x => x.UserGuid == userGuid).OrderBy(x => x.NormalizedName).ToListAsync();
    }

    public async Task<CategoryModel?> Get(int key, Guid userGuid)
    {
        return await _db.Category.SingleOrDefaultAsync(x => x.Id == key && x.UserGuid == userGuid);
    }

    public async Task<CategoryModel> Delete(int key, Guid userGuid)
    {
        var model = await _db.Category.SingleOrDefaultAsync(x => x.Id == key && x.UserGuid == userGuid);
        if (model is null)
            throw new KeyNotFoundException($"Model not found with id {key}");

        var activitiesReferencesActivity = await _db.Activity.Include(x => x.Category).AnyAsync(x => x.Category == model && x.Deleted == false);
        if (activitiesReferencesActivity)
            throw new InvalidOperationException($"It is not possible to delete a category which is still referenced by activities");

        model.Deleted = true;
        await _db.SaveChangesAsync();
        return model;
    }

    public async Task<CategoryModel> ChangeColor(int key, Color color, Guid userGuid)
    {
        var model = await _db.Category.SingleOrDefaultAsync(x => x.Id == key && x.UserGuid == userGuid);
        if (model is null)
            throw new KeyNotFoundException($"Model not found with id {key}");
        model.Color = color;
        await _db.SaveChangesAsync();
        return model;
    }
}
