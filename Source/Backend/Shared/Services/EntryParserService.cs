using database.Models;
using database.Persitance;
using Microsoft.EntityFrameworkCore;
using Shared.Services;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Shared.Services;

internal class EntryParserService : IEntryParserService
{
    private record ActivityCategory(string? Activity, string? Category);

    private readonly WeeklyReviewDbContext _db;
    private List<CategoryModel> _addedCats = new List<CategoryModel>();

    public EntryParserService(WeeklyReviewDbContext db)
    {
        _db = db;
    }

    public async Task<List<ActivityModel>> ParseEntry(List<string> activities, Guid userGuid)
    {
        var activityCategories = GetActivityCategories(activities);
        var res = await AddActivityCategories(activityCategories, userGuid);
        await _db.SaveChangesAsync();
        return res;
    }

    private async Task<List<ActivityModel>> AddActivityCategories(List<ActivityCategory> entires, Guid userGuid)
    {
        // Ensure all Categories and Activities exist
        // Retrive relvant Activities
        var res = new List<ActivityModel>();

        foreach (var entry in entires)
        {
            var key = entry.Category is null ? entry.Activity : entry.Category + ": " + entry.Activity;
            var act = await _db.Activity.SingleOrDefaultAsync(x => x.NormalizedName == key.ToLower() && x.UserGuid == userGuid);
            if (act is null)
                act = await CreateNewActivity(entry, userGuid);

            res.Add(act);
        }
        return res;
    }

    private async Task<ActivityModel> CreateNewActivity(ActivityCategory entry, Guid userGuid)
    {
        var act = entry.Category is null ? entry.Activity : entry.Category + ": " + entry.Activity;
        var cat = entry.Category;

        var catModel = cat is null ? null : await _db.Category.SingleOrDefaultAsync(x => x.NormalizedName == cat.ToLower() && x.UserGuid == userGuid);
        if (catModel is null)
            catModel = _addedCats.FirstOrDefault(x => x.Name.Equals(cat));

        if (string.IsNullOrEmpty(cat))
        {
            var defaultCat = await _db.Category.SingleOrDefaultAsync(x => x.UserGuid == userGuid && x.NormalizedName.Length == 0);
            if (defaultCat is null)
            {
                defaultCat = new CategoryModel("", 0, Color.White, userGuid);
                _db.Category.Add(defaultCat);
            }
            catModel = defaultCat;
        }
        else if (catModel is null)
        {
            catModel = new CategoryModel(cat, 0, Color.White, userGuid);
            _db.Category.Add(catModel);
            _addedCats.Add(catModel);
        }

        var actModel = new ActivityModel(act, false, catModel, userGuid);
        _db.Activity.Add(actModel);
        return actModel;
    }

    private List<ActivityCategory> GetActivityCategories(List<string> entries)
    {
        var res = new List<ActivityCategory>();
        foreach (var entry in entries)
        {
            var parseResult = ParseEntry(entry);

            if (parseResult.Category is not null && parseResult.Activity is null)
                throw new ArgumentException("An entry must contain an Activity, if a category is supplied");

            if (parseResult.Activity is not null &&
                res.FirstOrDefault(x => x.Activity == parseResult.Activity && x.Category == parseResult.Category) is null)
                res.Add(parseResult);
        }
        return res;
    }

    private ActivityCategory ParseEntry(string entry)
    {
        var splits = entry.Split(':');
        string? cat = null;
        string? act;
        if (splits.Count() > 2)
        {
            throw new ArgumentException("An entry cannot contain multiple ':'");
        }
        else if (splits.Count() == 2)
        {
            cat = splits[0];
            act = splits[1];
        }
        else
        {
            act = splits[0];
        }

        return new ActivityCategory(TrimSentance(act), TrimSentance(cat));
    }

    private string? TrimSentance(string? sentance)
    {
        if (sentance is null)
            return null;

        var regX = new Regex("\\s+");
        var res = regX.Replace(sentance, " ").Trim();
        if (res.Count() > 0)
            return res;
        else
            return null;
    }
}
