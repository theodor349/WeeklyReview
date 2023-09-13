using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WeeklyReview.Database.Models;
using WeeklyReview.Server.Controllers.Internal;
using WeeklyReview.Server.Persitance;
using WeeklyReview.Shared.Models;
using WeeklyReview.Shared.Services;

namespace WeeklyReview.Server.Controllers
{
    [ApiVersion("1.0")]
    public class EntryController : GenericAuthorizedApiController
    {
        private readonly IEntryService _entryService;

        public EntryController(IEntryService entryService)
        {
            _entryService = entryService;
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IEnumerable<EntryModel>> GetAll()
        {
            return Ok(_entryService.GetAll(UserGuid));
        }

        [HttpGet("{key}")]
        [EnableQuery]
        public ActionResult<EntryModel?> Get([FromRoute] int key)
        {
            return Ok(_entryService.Get(key, UserGuid));
        }

        [HttpPost("Enter")]
        public ActionResult<EntryModel?> Create([FromBody] EnterEntryModel model)
        {
            return Ok(_entryService.Create(model, UserGuid));
        }
    }
}
