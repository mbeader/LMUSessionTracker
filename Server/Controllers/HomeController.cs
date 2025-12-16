using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Tracking;
using LMUSessionTracker.Server.Models;
using LMUSessionTracker.Server.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LMUSessionTracker.Server.Controllers {
	public class HomeController : Controller {
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
			return View(vm);
		}

		public async Task<IActionResult> Session(string sessionId) {
			if(!ModelState.IsValid)
				return BadRequest();
			SessionViewModel vm = new SessionViewModel();
			vm.Session = await sessionRepo.GetSession(sessionId);
			vm.SessionState = await sessionRepo.GetSessionState(sessionId);
			Core.Tracking.Session session = await sessionObserver.GetSession(sessionId);
			vm.Standings = session?.LastStandings;
			vm.History = session?.History?.GetAllHistory();
			Dictionary<string, List<CarKey>> classes = new Dictionary<string, List<CarKey>>();
			if(vm.Standings != null) {
				foreach(Standing standings in vm.Standings) {
					if(!classes.TryGetValue(standings.carClass, out List<CarKey> cars)) {
						cars = new List<CarKey>();
						classes.Add(standings.carClass, cars);
					}
					cars.Add(new CarKey() { SlotId = standings.slotID, Veh = standings.vehicleFilename });
				}
			}
			foreach(string classname in classes.Keys)
				for(int i = 0; i < classes[classname].Count; i++)
					vm.PositionInClass.Add(classes[classname][i], i+1);
			vm.History?.ForEach(x => vm.Entries.Add(x.Key, x.Car));
			return View(vm);
		}

		public async Task<IActionResult> Laps(string sessionId, string carId) {
			if(!ModelState.IsValid)
				return BadRequest();
			CarHistory car = (await sessionObserver.GetSession(sessionId))?.History?.GetAllHistory().Find(x => x.Key.Matches(carId));
			if(car == null)
				return NotFound();
			return View(car);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error() {
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
