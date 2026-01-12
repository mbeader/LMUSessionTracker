using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Tracking;
using LMUSessionTracker.Server.Models;
using LMUSessionTracker.Server.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Controllers {
	[ApiController]
	[Route("api/[controller]/[action]")]
	public class HomeController : ControllerBase {
		private readonly ILogger<HomeController> logger;
		private readonly SessionRepository sessionRepo;
		private readonly SessionObserver sessionObserver;

		public HomeController(ILogger<HomeController> logger, SessionRepository sessionRepo, SessionObserver sessionObserver) {
			this.logger = logger;
			this.sessionRepo = sessionRepo;
			this.sessionObserver = sessionObserver;
		}

		public async Task<IActionResult> Index() {
			IndexViewModel vm = new IndexViewModel();
			vm.Sessions = await sessionRepo.GetSessions();
			List<SessionSummary> activeSessions = await sessionObserver.GetSessions();
			for(int i = 0; i < vm.Sessions.Count; i++) {
				SessionSummary activeSession = activeSessions.Find(x => x.SessionId == vm.Sessions[i].SessionId);
				if(activeSession != null) {
					activeSession.Timestamp = vm.Sessions[i].Timestamp;
					vm.Sessions[i] = activeSession;
				}
			}
			return Ok(vm);
		}

		public async Task<IActionResult> Session([Required] string sessionId) {
			if(!ModelState.IsValid)
				return BadRequest();
			SessionViewModel vm = new SessionViewModel();
			vm.Session = await sessionRepo.GetSession(sessionId);
			if(vm.Session == null)
				return NotFound();
			Core.Tracking.Session session = await sessionObserver.GetSession(sessionId);
			vm.SetSession(session);
			return Ok(vm);
		}

		public async Task<IActionResult> Laps([Required] string sessionId, [Required] string carId) {
			if(!ModelState.IsValid)
				return BadRequest();
			LapsViewModel vm = new LapsViewModel();
			vm.Session = await sessionObserver.GetSession(sessionId) ?? (await sessionRepo.GetSession(sessionId))?.To();
			if(vm.Session == null)
				return NotFound();
			vm.Car = vm.Session?.History.GetAllHistory().Find(x => x.Key.Matches(carId));
			if(vm.Car == null)
				return NotFound();
			return Ok(vm);
		}

		public async Task<IActionResult> EntryList([Required] string sessionId) {
			if(!ModelState.IsValid)
				return BadRequest();
			Core.Tracking.Session session = await sessionObserver.GetSession(sessionId) ?? (await sessionRepo.GetSession(sessionId))?.To();
			if(session == null)
				return NotFound();
			return Ok(await sessionRepo.GetEntries(sessionId));
		}

		public async Task<IActionResult> Tracks() {
			return Ok(await sessionRepo.GetTracks());
		}

		public async Task<IActionResult> BestLaps([FromQuery, Required] string track, [FromQuery] string network, [FromQuery] List<string> classes) {
			if(!ModelState.IsValid)
				return BadRequest();
			bool? onlineOnly = network == "online" ? true : network == "offline" ? false : null;
			classes ??= new List<string>();
			return Ok(await sessionRepo.GetLaps(track, onlineOnly, classes));
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() {
			return Ok(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
