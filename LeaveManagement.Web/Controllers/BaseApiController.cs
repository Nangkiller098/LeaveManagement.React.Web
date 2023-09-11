using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LeaveManagement.Web.Controllers
{  
    [ApiController]
    [Route("api/[controller]")]
   [ApiVersion("2.0")]
    public class BaseApiController : ControllerBase
    {
        
    }
}