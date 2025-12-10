using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Session;
using LMUSessionTracker.Core.Replay;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Services {
	public class ReplayService : PeriodicService<ReplayService> {
		private SessionManager manager;
		private List<(SessionInfo info, List<Standing> standings)> data;
		private ReplaySession replay;
		private int i;

		public ReplayService(ILogger<ReplayService> logger, IServiceProvider serviceProvider) : base(logger, serviceProvider) {
		}

		public override int CalculateDelay() {
			return 0;
		}

		public override Task Start(IServiceScope scope) {
			manager = scope.ServiceProvider.GetRequiredService<SessionManager>();
			data = Load("debug");
			replay = new ReplaySession();
			i = 0;
			return Task.CompletedTask;
		}

		public override Task<bool> Do() {
			if(i >= data.Count)
				return Task.FromResult(false);
			(SessionInfo info, List<Standing> standings) = data[i++];
			manager.UpdateSession(info);
			if(info != null) {
				replay.Update(standings);
				manager.UpdateStandings(standings);
			}
			manager.PeriodicPersist();
			return Task.FromResult(true);
		}

		public override Task End() {
			manager.Persist();
			foreach(CarReplay car in replay.Cars.Values)
				File.WriteAllText(Path.Combine("debug", "replay", $"replay-{car.Key.SlotId}.json"), JsonConvert.SerializeObject(car, Formatting.Indented));
			File.WriteAllText(Path.Combine("debug", "replay.json"), JsonConvert.SerializeObject(replay.Cars, Formatting.Indented));
			return Task.CompletedTask;
		}

		private List<(SessionInfo info, List<Standing> standings)> Load(string dir) {
			List<(SessionInfo info, List<Standing> standings)> data = new List<(SessionInfo info, List<Standing> standings)>();
			List<string> files = new List<string>(Directory.EnumerateFiles(dir));
			files.RemoveAll(x => x.Contains("replay"));
			files.Sort();
			for(int i = 0; i < files.Count;) try {
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
