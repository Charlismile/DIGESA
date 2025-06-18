using Microsoft.AspNetCore.Mvc;

namespace DIGESA.Controllers;

    [ApiController]
    [Route("api/[controller]")]
    public class PruebaController : ControllerBase
    {
        [HttpGet]
        public ActionResult<string> Get()
        {
            return "API funcionando";
        }

        [HttpGet("ping")]
        public ActionResult<string> Ping()
        {
            return "pong";
        }
    }
