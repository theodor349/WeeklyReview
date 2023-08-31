using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WeeklyReview.Server.Controllers.Internal
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GenericAuthorizedApiController : ControllerBase
    {
        public Guid UserGuid => Guid.NewGuid();

        public GenericAuthorizedApiController()
        {
        }
    }
}
