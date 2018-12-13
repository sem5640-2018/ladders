using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ladders.Controllers
{
    namespace challenges.Controllers.api
    {
        [Route("api/[controller]")]
        [ApiController]
        public class StatusController : ControllerBase
        {
            [HttpGet]
            public IActionResult GetStatus()
            {
                return Ok();
            }
        }
    }
}