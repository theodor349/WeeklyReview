using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace WeeklyReview.Server.Controllers.Internal
{
    //[Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GenericAuthorizedApiController : ODataController
    {
        public Guid UserGuid => new Guid("24fe9480-4e7a-4515-b96c-248171496591");

        public GenericAuthorizedApiController()
        {
        }
    }
}
