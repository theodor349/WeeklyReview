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
        private readonly IWeeklyReviewService _weeklyReviewService;
        private IActivityChangeService _activityChangeService => _weeklyReviewService.ActivityChange;

        public ActivityChangeController(IWeeklyReviewService weeklyReviewService)
        {
            _weeklyReviewService = weeklyReviewService;
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IEnumerable<CategoryModel>> GetAll()
        {
            return Ok(_activityChangeService.GetAll(UserGuid));
        }

        [HttpGet("{key}")]
        [EnableQuery]
        public ActionResult<CategoryModel> Get([FromRoute] int key)
        {
            return Ok(_activityChangeService.Get(key, UserGuid));
        }

        [HttpDelete("{key}")]
        public ActionResult<ActivityChangeModel> Delete([FromRoute] int key)
        {
            try
            {
                var model = _activityChangeService.Delete(key, UserGuid);
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
            _activityChangeService.RollBackActivityChange(key, UserGuid);
            return Ok();
        }
    }
}
