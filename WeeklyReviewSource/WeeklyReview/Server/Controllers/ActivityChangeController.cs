using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using WeeklyReview.Database.Models;
using WeeklyReview.Server.Controllers.Internal;
using WeeklyReview.Server.Persitance;
using WeeklyReview.Shared.Services;

namespace WeeklyReview.Server.Controllers
{
    public class ActivityChangeController : GenericAuthorizedApiController
    {
        private readonly WeeklyReviewApiDbContext _db;
        private readonly IActivityChangeService _rollBackService;

        public ActivityChangeController(WeeklyReviewApiDbContext db, IActivityChangeService rollBackService)
        {
            _db = db;
            _rollBackService = rollBackService;
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IEnumerable<CategoryModel>> GetAll()
        {
            return Ok(_db.ActivityChange.Where(x => x.UserGuid == UserGuid).AsQueryable());
        }

        [HttpGet("{key}")]
        [EnableQuery]
        public ActionResult<CategoryModel> Get([FromRoute] int key)
        {
            var res = _db.ActivityChange.SingleOrDefault(x => x.Id == key && x.UserGuid == UserGuid);
            return Ok(res);
        }

        [HttpDelete("{key}")]
        public ActionResult<ActivityModel> Delete([FromRoute] int key)
        {
            var model = _db.ActivityChange.SingleOrDefault(x => x.Id == key && x.UserGuid == UserGuid);
            if (model is null)
                return NotFound($"Model not found with id {key}");
            _db.ActivityChange.Remove(model);
            _db.SaveChanges();
            return Ok(model);
        }

        [HttpPost("{key}/Rollback")]
        public ActionResult Create([FromRoute] int key)
        {
            var model = _db.ActivityChange.SingleOrDefault(x => x.Id == key && x.UserGuid == UserGuid);
            if (model is null)
                return NotFound($"Model not found with id {key}");
            _rollBackService.RollBackActivityChange(model);
            return Ok();
        }
    }
}
