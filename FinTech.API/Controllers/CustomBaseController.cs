using FinTech.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinTech.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        [NonAction]
        public ActionResult CreateActionResultInstance<T>(CustomResponse<T> customResponse)
        {
            if (!customResponse.Succeeded)
            {
                if (customResponse.Args != null && customResponse.Args.Length > 0)
                    for (var i = 0; i < customResponse.Error.Details.Count; i++)
                        customResponse.Error.Details[i] = string.Format(customResponse.Error.Details[i], customResponse.Args);
            }

            if (customResponse.StatusCode == 204)
            {
                return new ObjectResult(null)
                {
                    StatusCode = customResponse.StatusCode
                };
            }

            return new ObjectResult(customResponse)
            {
                StatusCode = customResponse.StatusCode
            };
        }
    }
}
