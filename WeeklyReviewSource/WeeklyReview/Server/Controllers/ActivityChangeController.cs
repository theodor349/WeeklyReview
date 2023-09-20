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
        public async Task<ActionResult<IEnumerable<CategoryModel>>> GetAll()
        {
            return Ok(await _activityChangeService.GetAll(UserGuid));
        }

        [HttpGet("{key}")]
        [EnableQuery]
        public async Task<ActionResult<CategoryModel>> Get([FromRoute] int key)
        {
            return Ok(await _activityChangeService.Get(key, UserGuid));
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult<ActivityChangeModel>> Delete([FromRoute] int key)
        {
            try
            {
                var model = await _activityChangeService.Delete(key, UserGuid);
                return Ok(model);
            }
            catch(KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpPost("{key}/Rollback")]
        public async Task<ActionResult> Rollback([FromRoute] int key)
        {
            await _activityChangeService.RollBackActivityChange(key, UserGuid);
            return Ok();
        }
    }
}
