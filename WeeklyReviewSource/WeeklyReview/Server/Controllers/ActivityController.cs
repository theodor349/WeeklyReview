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
        public ActionResult<IEnumerable<ActivityModel>> GetAll()
        {
            return Ok(_activityService.GetAll(UserGuid));
        }

        [HttpGet("{key}")]
        [EnableQuery]
        public ActionResult<ActivityModel> Get([FromRoute] int key)
        {
            return Ok(_activityService.Get(key, UserGuid));
        }

        [HttpDelete("{key}")]
        public ActionResult<ActivityModel> Delete([FromRoute] int key)
        {
            return Ok(_activityService.Delete(key, UserGuid));
        }

        [HttpPost("{sKey}/ChangeTo/{dKey}")]
        public ActionResult<ActivityChangeModel> Create([FromRoute] int sKey, [FromRoute] int dKey)
        {
            return Ok(_activityService.Convert(sKey, dKey, UserGuid));
        }
    }
}
