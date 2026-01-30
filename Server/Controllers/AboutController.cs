using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LMUSessionTracker.Server.Controllers {
	[ApiController]
	[Route("api/[controller]")]
	public class AboutController : ControllerBase {
		private readonly ILogger<AboutController> logger;
		private readonly AboutOptions options;

		public AboutController(ILogger<AboutController> logger, IOptions<AboutOptions> options) {
			this.logger = logger;
			this.options = options.Value;
		}

		public IActionResult About() {
			return Ok(options);
		}
	}
}
