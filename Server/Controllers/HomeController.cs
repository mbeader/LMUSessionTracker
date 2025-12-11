using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Session;
using LMUSessionTracker.Server.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;

namespace LMUSessionTracker.Server.Controllers {
	public class HomeController : Controller {
		private readonly ILogger<HomeController> _logger;
		private readonly SessionViewer viewer;

		public HomeController(ILogger<HomeController> logger, SessionViewer viewer) {
			_logger = logger;
			this.viewer = viewer;
		}

		public IActionResult Index() {
			IndexViewModel vm = new IndexViewModel() { Info = viewer.Info, Standings = viewer.Standings, History = viewer.History.GetAllHistory() };
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
			return View(vm);
		}

		public IActionResult Laps(string id) {
			CarHistory car = viewer.History?.GetAllHistory().Find(x => x.Key.Matches(id));
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
