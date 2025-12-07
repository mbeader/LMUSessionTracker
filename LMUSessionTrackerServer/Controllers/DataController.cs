using LMUSessionTracker.Core.Protocol;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Controllers {
	[ApiController]
	[Route("api/[controller]")]
	public class DataController : ControllerBase {
		private readonly ILogger<DataController> _logger;
		private readonly ProtocolServer arbiter;

		public DataController(ILogger<DataController> logger, ProtocolServer arbiter) {
			_logger = logger;
			this.arbiter = arbiter;
		}

		[HttpPost]
		public async Task<IActionResult> Index([FromBody] ProtocolMessage data) {
			if(IPAddress.IsLoopback(HttpContext.Connection.RemoteIpAddress))
				return StatusCode(403);
			if(!ModelState.IsValid)
				return BadRequest();
			return Ok(await arbiter.Receive(data));
		}
	}
}
