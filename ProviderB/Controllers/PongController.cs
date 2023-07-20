using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ProviderB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PongController : ControllerBase
    {

        [HttpGet("pong")]
        public string PongService()
        {
            return "Pong - ProviderB";
        }
    }
}
