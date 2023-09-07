using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Data.SqlClient;
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
        private readonly WeeklyReviewApiDbContext _db;
        private readonly INewEntryAdderService _entryAdderService;
        private readonly INewEntryParserService _entryParserService;

        public EntryController(WeeklyReviewApiDbContext db, INewEntryAdderService entryAdderService, INewEntryParserService entryParserService)
        {
            _db = db;
            _entryAdderService = entryAdderService;
            _entryParserService = entryParserService;
        }

        [HttpGet]
        [EnableQuery]
        public ActionResult<IEnumerable<EntryModel>> GetAll()
        {
            return Ok(_db.Entry.Where(x => x.UserGuid == UserGuid).AsQueryable());
        }

        [HttpGet("{key}")]
        [EnableQuery]
        public ActionResult<EntryModel> Get([FromRoute] int key)
        {
            var res = _db.Entry.SingleOrDefault(x => x.Id == key && x.UserGuid == UserGuid);
            return Ok(res);
        }

        [HttpPost("Enter")]
        public ActionResult<EntryModel> Create([FromBody] EnterEntryModel model)
        {
            var activities = _entryParserService.ParseEntry(model.Entries, UserGuid);
            var entry = _entryAdderService.AddEntry(model.Date, activities, UserGuid);
            return Ok(entry);
        }
    }
}
