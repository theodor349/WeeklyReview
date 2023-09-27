using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Syncfusion.Blazor.Diagrams;
using WeeklyReview.Database.Models;
using WeeklyReview.Server.Controllers.Internal;
using WeeklyReview.Server.Persitance;
using WeeklyReview.Shared.Services;

namespace WeeklyReview.Server.Controllers
{
    [ApiVersion("1.0")]
    public class ActivityController : GenericAuthorizedApiController
    {
        private readonly IWeeklyReviewService _weeklyReviewService;
        private IActivityService _activityService => _weeklyReviewService.Activity;

        public ActivityController(IWeeklyReviewService weeklyReviewService)
        {
            _weeklyReviewService = weeklyReviewService;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<ActivityModel>>> GetAll()
        {
            return Ok(await _activityService.GetAll(UserGuid));
        }

        [HttpGet("{key}")]
        [EnableQuery]
        public async Task<ActionResult<ActivityModel>> Get([FromRoute] int key)
        {
            return Ok(await _activityService.Get(key, UserGuid));
        }

        [HttpDelete("{key}")]
        public async Task<ActionResult<ActivityModel>> Delete([FromRoute] int key)
        {
            return Ok(await _activityService.Delete(key, UserGuid));
        }

        [HttpPost("{sKey}/ChangeTo/{dKey}")]
        public async Task<ActionResult<ActivityChangeModel>> Create([FromRoute] int sKey, [FromRoute] int dKey)
        {
            return Ok(await _activityService.Convert(sKey, dKey, UserGuid));
        }
    }
}
