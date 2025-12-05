using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Session;
using LMUSessionTracker.Core.Session.Replay;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Services {
	public class ReplayService : BackgroundService {
		private readonly ILogger<SessionService> logger;
		private readonly IServiceProvider serviceProvider;

		public ReplayService(ILogger<SessionService> logger, IServiceProvider serviceProvider) {
			this.logger = logger;
			this.serviceProvider = serviceProvider;
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken) {
			logger.LogInformation("Starting service");
			using(IServiceScope scope = serviceProvider.CreateScope()) {
				SessionManager manager = scope.ServiceProvider.GetRequiredService<SessionManager>();
				List<(SessionInfo info, List<Standing> standings)> data = Load("debug");
				Replay replay = new Replay();
				int i = 0;
				while(!stoppingToken.IsCancellationRequested) {
					if(i >= data.Count)
						break;
					(SessionInfo info, List<Standing> standings) = data[i++];
					manager.UpdateSession(info);
					if(info != null) {
						replay.Update(standings);
						manager.UpdateStandings(standings);
					}
					manager.PeriodicPersist();
				}
				manager.Persist();
				logger.LogInformation("Stopping service");
				foreach(CarReplay car in replay.Cars.Values)
					File.WriteAllText(Path.Combine("debug", "replay", $"replay-{car.Key.SlotId}.json"), JsonConvert.SerializeObject(car, Formatting.Indented));
				File.WriteAllText(Path.Combine("debug", "replay.json"), JsonConvert.SerializeObject(replay.Cars, Formatting.Indented));
			}
			return Task.CompletedTask;
		}

		private List<(SessionInfo info, List<Standing> standings)> Load(string dir) {
			List<(SessionInfo info, List<Standing> standings)> data = new List<(SessionInfo info, List<Standing> standings)>();
			List<string> files = new List<string>(Directory.EnumerateFiles(dir));
			files.RemoveAll(x => x.Contains("replay"));
			files.Sort();
			for(int i = 0; i < files.Count;) 				try {
					SessionInfo info = JsonConvert.DeserializeObject<SessionInfo>(File.ReadAllText(files[i++]));
					List<Standing> standings = JsonConvert.DeserializeObject<List<Standing>>(File.ReadAllText(files[i++]));
					data.Add((info, standings));
				} catch(Exception e) {
					logger.LogCritical(e, "Load failed");
					throw;
				}
			return data;
		}
	}
}
