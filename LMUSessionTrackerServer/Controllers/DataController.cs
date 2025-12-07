using LMUSessionTracker.Core.Protocol;
using LMUSessionTracker.Core.Session;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Controllers {
	[ApiController]
	[Route("api/[controller]")]
	public class DataController : ControllerBase {
		private readonly ILogger<DataController> _logger;
		private readonly SessionArbiter arbiter;

		public DataController(ILogger<DataController> logger, SessionArbiter arbiter) {
			_logger = logger;
			this.arbiter = arbiter;
		}

		[HttpPost()]
		public async Task<IActionResult> Index([FromBody] ProtocolMessage data) {
			if(!ModelState.IsValid)
				return BadRequest();
			return Ok(await arbiter.Receive(data));
		}
	}
}
