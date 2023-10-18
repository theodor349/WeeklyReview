using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WeeklyReview.Database.Models;
using WeeklyReview.Server.Controllers.Internal;
using WeeklyReview.Server.Persitance;
using WeeklyReview.Shared.Models;
using WeeklyReview.Shared.Models.DTOs;
using WeeklyReview.Shared.Services;

namespace WeeklyReview.Server.Controllers
{
    [ApiVersion("1.0")]
    public class EntryController : GenericAuthorizedApiController
    {
        private readonly IWeeklyReviewService _weeklyReviewService;
        private IEntryService _entryService => _weeklyReviewService.Entry;

        public EntryController(IWeeklyReviewService weeklyReviewService)
        {
            _weeklyReviewService = weeklyReviewService;
        }

        [HttpGet]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<EntryModel>>> GetAll()
        {
            return Ok(await _entryService.GetAll(UserGuid));
        }

        [HttpGet("{key}")]
        [EnableQuery]
        public async Task<ActionResult<EntryModel?>> Get([FromRoute] int key)
        {
            return Ok(await _entryService.Get(key, UserGuid));
        }

        [HttpPut("around")]
        [EnableQuery]
        public async Task<ActionResult<EntryModel?>> GetAllAround([FromBody] GetEntriesAroundDto model)
        {
            return Ok(await _entryService.GetAllAroundDate(UserGuid, model.Date, model.DaysAround));
        }

        [HttpPost("Enter")]
        public async Task<ActionResult<EntryModel?>> Create([FromBody] EnterEntryModel model)
        {
            try
            {
                return Ok(await _entryService.Create(model, UserGuid));
            }
            catch(ArgumentException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
