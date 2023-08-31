using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WeeklyReview.Database.Models;
using WeeklyReview.Server.Controllers.Internal;
using WeeklyReview.Shared.Services;

namespace WeeklyReview.Server.Controllers
{
    [ApiVersion("1.0")]
    public class EntryController : GenericAuthorizedApiController
    {
        private readonly INewEntryAdderService _entryAdderService;
        private readonly INewEntryParserService _entryParserService;

        public EntryController(INewEntryAdderService entryAdderService, INewEntryParserService entryParserService)
        {
            _entryAdderService = entryAdderService;
            _entryParserService = entryParserService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EntryModel))]
        public async Task<IActionResult> Create(List<string> entries, DateTime date)
        {
            try
            {
                var activities = _entryParserService.ParseEntry(entries, UserGuid);
                var entry = _entryAdderService.AddEntry(date, activities, UserGuid);
                return Ok(entry);
            }
            catch (SqlException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
