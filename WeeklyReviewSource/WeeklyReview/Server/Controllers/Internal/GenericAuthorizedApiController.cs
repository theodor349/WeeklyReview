using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WeeklyReview.Server.Controllers.Internal
{
    //[Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GenericAuthorizedApiController : ControllerBase
    {
        public Guid UserGuid => new Guid("24fe9480-4e7a-4515-b96c-248171496591");

        public GenericAuthorizedApiController()
        {
        }
    }
}
