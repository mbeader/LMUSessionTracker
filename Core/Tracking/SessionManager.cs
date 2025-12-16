using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class SessionManager {
		private static readonly TimeSpan writeDelay = new TimeSpan(0, 0, 10);
		private readonly ILogger<SessionService> logger;
		private readonly History history;
		private string sessionId;
		private SessionInfo lastInfo;
		private List<Standing> lastStandings;
		private bool isCurrent = false;
		private DateTime lastWrite;

		public bool IsCurrent => isCurrent;
		public SessionInfo Info => lastInfo;
		public List<Standing> Standings => lastStandings;
		public History History => history;

		public SessionManager(ILogger<SessionService> logger) {
			this.logger = logger;
			ReadResult<SessionState> result = SessionState.Read();
			if(!result.Success)
				logger.LogError(result.Exception, "Failed to read state");
			SessionState state = result.Value;
			if(state != null) {
				sessionId = state.SessionId;
				lastInfo = state.Info;
				history = new History(state.History);
				logger.LogInformation($"Loaded session: {sessionId}");
			}
			history ??= new History();
			sessionId = CreateSession(sessionId);
			lastWrite = DateTime.UtcNow;
		}

		public bool UpdateSession(SessionInfo sessionInfo) {
			if(sessionInfo == null) {
				if(isCurrent) {
					Persist();
					isCurrent = false;
					logger.LogInformation($"Inactive session: {sessionId}");
				}
				return false;
			}
			if(lastInfo != null && IsNewSession(sessionInfo)) {
				Persist();
				Reset();
				logger.LogInformation($"End session: {sessionId}");
				sessionId = CreateSession();
				logger.LogInformation($"Start session: {sessionId}");
			} else if(lastInfo == null) {
				Reset();
				logger.LogInformation($"End session: {sessionId}");
				sessionId = CreateSession();
				logger.LogInformation($"Start session: {sessionId}");
			}
			if(isCurrent == false)
				logger.LogInformation($"Active session: {sessionId}");
			lastInfo = sessionInfo;
			isCurrent = true;
			return true;
		}

		private void Reset() {
			lastInfo = null;
			lastStandings = null;
			history.Clear();
			logger.LogInformation($"Reset");
		}

		private bool IsNewSession(SessionInfo sessionInfo) {
			if(sessionInfo == null)
				return false;
			else if(lastInfo == null && sessionInfo != null)
				return true;
			else if(lastInfo != null && sessionInfo != null && (
				sessionInfo.trackName != lastInfo.trackName ||
				sessionInfo.session != lastInfo.session ||
				sessionInfo.raceCompletion.timeCompletion != -1 && sessionInfo.raceCompletion.timeCompletion < lastInfo.raceCompletion.timeCompletion
			))
				return true;
			return false;
		}

		public bool UpdateStandings(List<Standing> standings) {
			if(standings != null) {
				standings.ForEach(x => {
					if(!string.IsNullOrEmpty(x.carNumber) && x.carNumber.EndsWith('\u0000'))
						x.carNumber = x.carNumber.TrimEnd('\u0000');
				});
				standings.Sort((a, b) => a.position.CompareTo(b.position));
				lastStandings = standings;
				history.Update(standings, DateTime.UtcNow);
			} else
				return false;
			return true;
		}

		private string CreateSession(string sessionId = null) {
			string ts = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
			string guid;
			if(sessionId != null)
				guid = sessionId.Substring(sessionId.IndexOf('-') + 1);
			else
				guid = Guid.NewGuid().ToString();
			return $"{ts}-{guid}";
		}

		public void Persist() {
			if(isCurrent && lastInfo != null) {
				WriteResult result = SessionState.Write(sessionId, lastInfo, history.GetAllHistory());
				if(!result.Success)
					logger.LogError(result.Exception, "Failed to write state");
				lastWrite = DateTime.UtcNow;
			}
		}

		public void PeriodicPersist() {
			if(isCurrent && DateTime.UtcNow - lastWrite > writeDelay)
				Persist();
		}

		public void CombineHistories(List<string> sessionIds) {
			if(sessionIds.Count == 0)
				return;
			ReadResult<SessionState> readResult = SessionState.Read(sessionIds[0]);
			if(!readResult.Success || readResult.Value == null)
				throw new Exception("Failed to combine", readResult.Exception);
			Dictionary<CarKey, CarHistory> combined = new Dictionary<CarKey, CarHistory>();
			if(readResult.Value.History != null)
				foreach(CarHistory car in readResult.Value.History)
					combined.Add(car.Key, car);
			for(int i = 1; i < sessionIds.Count; i++) {
				readResult = SessionState.Read(sessionIds[i]);
				if(!readResult.Success || readResult.Value == null)
					throw new Exception("Failed to combine", readResult.Exception);
				if(readResult.Value.History != null)
					foreach(CarHistory car in readResult.Value.History)
						if(combined.TryGetValue(car.Key, out CarHistory existing))
							existing.Combine(car);
						else
							combined.Add(car.Key, car);

			}
			string sessionId = $"{DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}-{sessionIds[^1][18..]}";
			SessionState.Write(sessionId, readResult.Value.Info, new List<CarHistory>(combined.Values));
		}
	}
}
