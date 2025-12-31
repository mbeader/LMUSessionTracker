using LMUSessionTracker.Core.Protocol;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Controllers {
	[ApiController]
	[Route("api/[controller]")]
	public class DataController : ControllerBase {
		private readonly ILogger<DataController> _logger;
		private readonly ProtocolServer arbiter;
		private readonly ProtocolAuthenticator authenticator;

		public DataController(ILogger<DataController> logger, ProtocolServer arbiter, ProtocolAuthenticator authenticator) {
			_logger = logger;
			this.arbiter = arbiter;
			this.authenticator = authenticator;
		}

		[HttpPost]
		public async Task<IActionResult> Index([FromBody] ProtocolMessage data) {
			if(!ModelState.IsValid)
				return BadRequest();
			bool? verified = await authenticator.Verify(Request, data);
			if(!verified.HasValue)
				return StatusCode(401);
			if(verified.Value)
				return Ok(await arbiter.Receive(data));
			else
				return StatusCode(403);
		}

		[HttpPost("{action}")]
		public async Task<IActionResult> Authenticate([FromBody] ProtocolCredential credential) {
			if(!ModelState.IsValid)
				return BadRequest();
			return Ok(await authenticator.Authenticate(Request, credential));
		}
	}
}
