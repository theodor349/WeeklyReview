using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using System.Security.Claims;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;


namespace WeeklyReview.Server.Controllers.Internal
{
    //[Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class GenericAuthorizedApiController : ODataController
    {
        public Guid UserGuid
        {
            get
            {
                //var guidString = User.Claims.First(x => x.Type.Contains("objectidentifier")).Value;
                //var userGuid = new Guid(guidString);
                //return userGuid;

                return new Guid("11D18C23-0F97-47AC-8F8C-2CCAE411EE14");
            }
        }

        public Guid GetUserGuid(string email)
        {
            if (email.Equals("theodor349@gmail.com", StringComparison.OrdinalIgnoreCase))
                return new Guid("99D18C23-0F97-47AC-8F8C-2CCAE411EE14");
            else 
                return new Guid("11D18C23-0F97-47AC-8F8C-2CCAE411EE14");
        }

        public GenericAuthorizedApiController()
        {
        }
    }
}
