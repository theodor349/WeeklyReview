using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Data.SqlClient;
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
        private readonly WeeklyReviewApiDbContext _db;
        private readonly IActivityChangeService _activityChangeService;

        public ActivityController(WeeklyReviewApiDbContext db, IActivityChangeService activityChangeService)
        {
            _db = db;
            _activityChangeService = activityChangeService;
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IEnumerable<ActivityModel>> GetAll()
        {
            return Ok(_db.Activity.Where(x => x.UserGuid == UserGuid).AsQueryable());
        }

        [HttpGet("{key}")]
        [EnableQuery]
        public ActionResult<ActivityModel> Get([FromRoute] int key)
        {
            var res = _db.Activity.SingleOrDefault(x => x.Id == key && x.UserGuid == UserGuid);
            return Ok(res);
        }

        [HttpDelete("{key}")]
        public ActionResult<ActivityModel> Delete([FromRoute] int key)
        {
            var model = _db.Activity.SingleOrDefault(x => x.Id == key && x.UserGuid == UserGuid);
            if (model is null)
                return NotFound($"Model not found with id {key}");
            model.Deleted = true;
            _db.SaveChanges();
            return Ok(model);
        }

        [HttpPost("{sKey}/ChangeTo/{dKey}")]
        public ActionResult<ActivityChangeModel> Create([FromRoute] int sKey, [FromRoute] int dKey)
        {
            var sModel = _db.Activity.SingleOrDefault(x => x.Id == sKey);
            var dModel = _db.Activity.SingleOrDefault(x => x.Id == dKey);
            if (sModel is null)
                return NotFound($"Model not found with id {sKey}");
            if (dModel is null)
                return NotFound($"Model not found with id {dKey}");
            var model = _activityChangeService.ChangeActivity(sModel, dModel, UserGuid);
            return Ok(model);
        }
    }
}
