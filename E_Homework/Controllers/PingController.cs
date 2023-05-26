using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Homework.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PingController : ControllerBase
    {
        private readonly ILogger<PingController> _logger;

        public PingController(ILogger<PingController> logger)
        {
            _logger = logger;
        }

        // GET api/<PingController>
        [HttpGet()]
        [AllowAnonymous]
        public string Get()
        {
            var Name = System.Reflection.Assembly.GetExecutingAssembly().GetName();
            string appversion = Name.Version?.ToString() ?? "7.0";
            _logger.LogTrace($"Called Ping returned {appversion}");
            string appname = Name.Name?.ToString() ?? "E_Homework";
            return $"pong from {appname} at {DateTime.UtcNow} App Version : {appversion}";
        }

    }
}
