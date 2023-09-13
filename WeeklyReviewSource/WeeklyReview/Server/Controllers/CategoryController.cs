using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using WeeklyReview.Database.Models;
using WeeklyReview.Server.Controllers.Internal;
using WeeklyReview.Server.Persitance;

namespace WeeklyReview.Server.Controllers
{
    public class CategoryController : GenericAuthorizedApiController
    {
        private readonly WeeklyReviewApiDbContext _db;

        public CategoryController(WeeklyReviewApiDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IEnumerable<CategoryModel>> GetAll()
        {
            return Ok(_db.Category.Where(x => x.UserGuid == UserGuid).AsQueryable());
        }

        [HttpGet("{key}")]
        [EnableQuery]
        public ActionResult<CategoryModel> Get([FromRoute] int key)
        {
            var res = _db.Category.SingleOrDefault(x => x.Id == key && x.UserGuid == UserGuid);
            return Ok(res);
        }

        [HttpDelete("{key}")]
        public ActionResult<ActivityModel> Delete([FromRoute] int key)
        {
            var model = _db.Category.SingleOrDefault(x => x.Id == key && x.UserGuid == UserGuid);
            if (model is null)
                return NotFound($"Model not found with id {key}");

            var activitiesReferencesActivity = _db.Activity.Include(x => x.Category).Any(x => x.Category == model && x.Deleted == false);
            if (activitiesReferencesActivity)
                return BadRequest($"It is not possible to delete a category which is still referenced by activities");

            model.Deleted = true;
            _db.SaveChanges();
            return Ok(model);
        }

        [HttpPost("{key}/ChangeColor")]
        public ActionResult<ActivityChangeModel> ChangeColor([FromRoute] int key, [FromBody] Color color)
        {
            var model = _db.Category.SingleOrDefault(x => x.Id == key);
            if (model is null)
                return NotFound($"Model not found with id {key}");
            model.Color = color;
            _db.SaveChanges();
            return Ok(model);
        }
    }
}
