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
        private readonly IWeeklyReviewService _weeklyReviewService;

        public ActivityChangeController(WeeklyReviewApiDbContext db, IWeeklyReviewService weeklyReviewService)
        {
            _db = db;
            _weeklyReviewService = weeklyReviewService;
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IEnumerable<CategoryModel>> GetAll()
        {
            return Ok(_weeklyReviewService.ActivityChange.GetAll(UserGuid));
        }

        [HttpGet("{key}")]
        [EnableQuery]
        public ActionResult<CategoryModel> Get([FromRoute] int key)
        {
            return Ok(_weeklyReviewService.ActivityChange.Get(key, UserGuid));
        }

        [HttpDelete("{key}")]
        public ActionResult<ActivityChangeModel> Delete([FromRoute] int key)
        {
            try
            {
                var model = _weeklyReviewService.ActivityChange.Remove(key, UserGuid);
                return Ok(model);
            }
            catch(KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("{key}/Rollback")]
        public ActionResult Rollback([FromRoute] int key)
        {
            var model = _db.ActivityChange.SingleOrDefault(x => x.Id == key && x.UserGuid == UserGuid);
            if (model is null)
                return NotFound($"Model not found with id {key}");
            _weeklyReviewService.ActivityChange.RollBackActivityChange(model);
            return Ok();
        }
    }
}
