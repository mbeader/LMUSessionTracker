using LMUSessionTracker.Core.LMU;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class Session {
		private static readonly double fuzziness = 5;
		private static readonly TimeSpan primarySwapLimit = new TimeSpan(0, 0, 10);
		private DateTime lastPromotion;

		public string SessionId { get; private set; }
		public string PrimaryClientId { get; private set; }
		public List<string> SecondaryClientIds { get; private set; }
		public string Track { get; private set; }
		public string Type { get; private set; }
		public bool Online { get; private set; }
		public Dictionary<string, bool> RoleChanges { get; private set; }
		public SessionInfo LastInfo { get; private set; }
		public List<Standing> LastStandings { get; private set; }
		public EntryList Entries { get; private set; }
		public History History { get; private set; }
		public DateTime Timestamp { get; private set; }
		public DateTime LastUpdate { get; private set; }
		public bool Finished { get; private set; }
		public bool Closed { get; private set; }

		private Session(string sessionId, SessionInfo info, DateTime timestamp, EntryList entries, List<CarHistory> history) {
			SessionId = sessionId;
			SecondaryClientIds = new List<string>();
			Track = info.trackName;
			Type = info.session;
			RoleChanges = new Dictionary<string, bool>();
			LastInfo = info;
			Entries = entries;
			History = new History(history, entries);
			Timestamp = timestamp;
			LastUpdate = timestamp;
			Finished = IsFinished(info);
		}

		public static Session Create(string sessionId, SessionInfo info, DateTime timestamp, MultiplayerTeams teams = null, List<CarHistory> history = null) {
			EntryList entries = new EntryList(teams);
			return new Session(sessionId, info, timestamp, entries, history) {
				Online = teams != null
			};
		}

		public static Session Create(string sessionId, SessionInfo info, DateTime timestamp, EntryList entries, List<CarHistory> history) {
			return new Session(sessionId, info, timestamp, entries, history) {
				Online = entries != null
			};
		}

		public bool HasClient() {
			return PrimaryClientId != null && SecondaryClientIds.Count > 0;
		}

		public bool RegisterClient(string clientId) {
			if(string.IsNullOrEmpty(clientId))
				throw new ArgumentException("Cannot be null or empty", nameof(clientId));
			if(PrimaryClientId != clientId || !SecondaryClientIds.Contains(clientId)) {
				if(PrimaryClientId == null) {
					PrimaryClientId = clientId;
				} else {
					SecondaryClientIds.Add(clientId);
				}
			}
			return PrimaryClientId == clientId;
		}

		public void UnregisterClient(string clientId) {
			if(string.IsNullOrEmpty(clientId))
				throw new ArgumentException("Cannot be null or empty", nameof(clientId));
			if(PrimaryClientId == clientId) {
				if(SecondaryClientIds.Count > 0) {
					PrimaryClientId = SecondaryClientIds[0];
					SecondaryClientIds.RemoveAt(0);
					ChangeRole(PrimaryClientId, true);
				} else {
					PrimaryClientId = null;
				}
			} else {
				SecondaryClientIds.Remove(clientId);
			}
		}

		private void ChangeRole(string clientId, bool isPrimary) {
			if(RoleChanges.TryGetValue(clientId, out bool isPrimaryExisting)) {
				if(isPrimaryExisting != isPrimary)
					RoleChanges[clientId] = isPrimary;
			} else
				RoleChanges.Add(clientId, isPrimary);
		}

		private void SwapPrimary(string clientId) {
			if(SecondaryClientIds.Count == 0)
				return;
			int index = SecondaryClientIds.FindIndex(x => x == clientId);
			string prevPrimary = PrimaryClientId;
			PrimaryClientId = SecondaryClientIds[index];
			SecondaryClientIds.RemoveAt(index);
			SecondaryClientIds.Add(prevPrimary);
			ChangeRole(prevPrimary, false);
			ChangeRole(PrimaryClientId, true);
		}

		public void CheckForPromotion(string clientId, DateTime timestamp) {
			bool isPrimary = IsPrimary(clientId);
			bool isSecondary = !isPrimary && IsSecondary(clientId);
			if(isPrimary || !isSecondary || RoleChanges.ContainsKey(clientId))
				return;
			if(timestamp - LastUpdate > primarySwapLimit && timestamp - lastPromotion > primarySwapLimit) {
				SwapPrimary(clientId);
				lastPromotion = timestamp;
			}
		}

		public bool? AcknowledgeRole(string clientId) {
			if(RoleChanges.TryGetValue(clientId, out bool isPrimary)) {
				RoleChanges.Remove(clientId);
				return isPrimary;
			} else
				return null;
		}

		public bool IsPrimary(string clientId) => PrimaryClientId == clientId;

		public bool IsSecondary(string clientId) => SecondaryClientIds.Contains(clientId);

		public bool Update(SessionInfo info, List<Standing> standings, MultiplayerTeams teams, DateTime timestamp) {
			LastInfo = info ?? LastInfo;
			LastStandings = standings ?? LastStandings;
			if(standings != null) {
				standings.Sort((a, b) => a.position.CompareTo(b.position));
				History.Update(standings, timestamp);
			}
			LastUpdate = timestamp;
			Finished = IsFinished(info);
			if(teams != null) {
				EntryList entries = new EntryList(teams);
				History.UpdateCars(entries);
				return Entries.Merge(entries);
			}
			return false;
		}

		public List<string> Close() {
			Finished = true;
			Closed = true;
			List<string> clients = new List<string>();
			if(PrimaryClientId != null)
				clients.Add(PrimaryClientId);
			clients.AddRange(SecondaryClientIds);
			return clients;
		}

		private bool IsFinished(SessionInfo info) {
			return Finished || (info != null && info.gamePhase == (int)GamePhase.Checkered);
		}

		public SessionDiff IsSameSession(SessionInfo info, MultiplayerTeams teams = null) {
			SessionDiff diff = new SessionDiff() { SessionId = SessionId, Difference = SessionDifference.None, MessageFormat = "Expected: {0}. Actual {1}.", MessageParams = new object[2] };
			if(Track != info.trackName) {
				diff.Set(SessionDifference.Track, Track, info.trackName);
			} else if(Type != info.session) {
				diff.Set(SessionDifference.Type, Type, info.session);
			} else {
				bool completionOk = CompletionNotDecreased(info);
				bool currentTimeOk = IsWithinFuzziness(LastInfo.currentEventTime, info.currentEventTime);
				bool remainingTimeOk = IsWithinFuzziness(LastInfo.timeRemainingInGamePhase, info.timeRemainingInGamePhase);
				if(!completionOk && (!currentTimeOk || !remainingTimeOk))
					diff.Set(SessionDifference.Completion,
						(LastInfo.raceCompletion?.timeCompletion, LastInfo.currentEventTime, LastInfo.timeRemainingInGamePhase),
						(info.raceCompletion?.timeCompletion, info.currentEventTime, info.timeRemainingInGamePhase));
				else if(!IsValidPhaseTransition(info)) {
					diff.Difference = SessionDifference.PhaseTransition;
					diff.MessageFormat = "{0} to {1}{2}";
					diff.MessageParams = new object[] { LastInfo.gamePhase, info.gamePhase, Finished ? "while finished" : "" };
				}
			}
			if(diff.Difference == SessionDifference.None) {
				if(Online != (teams != null)) {
					diff.Set(SessionDifference.Network, Online ? "online" : "offline", teams != null ? "online" : "offline");
				} else if(!Entries.HasAnyMatch(new EntryList(teams))) {
					diff.Difference = SessionDifference.EntryList;
					diff.MessageFormat = "Entrylist has no match";
				}
			}
			//bool same = Track == info.trackName &&
			//	Type == info.session && (
			//		CompletionNotDecreased(info) || (
			//			IsWithinFuzziness(LastInfo.currentEventTime, info.currentEventTime) &&
			//			IsWithinFuzziness(LastInfo.timeRemainingInGamePhase, info.timeRemainingInGamePhase))) &&
			//	IsValidPhaseTransition(info);
			//if(!same || Online != (teams != null))
			//	return false;
			//return Entries.HasAnyMatch(new EntryList(teams));
			return diff;
		}

		private bool CompletionNotDecreased(SessionInfo info) {
			if(LastInfo.raceCompletion == null || info.raceCompletion == null)
				return true;
			double last = LastInfo.raceCompletion.timeCompletion;
			double curr = info.raceCompletion.timeCompletion;
			return last == -1 || curr == -1 || last <= curr;
		}

		private bool IsWithinFuzziness(double last, double curr) {
			return Math.Abs(curr - last) <= fuzziness;
		}

		/// <summary>
		/// For this purpose, valid transitions are:<br/>
		///	&gt; no change<br/>
		///	&gt; advancing anywhere in starting sequence from starting to green (or a main phase)<br/>
		///	&gt; changing between any two main phases<br/>
		///	&gt; between checkered and paused but only between these two once checkered the first time<br/>
		///	&gt; between paused and anything else (technically this has potential to go backwards in starting sequence)<br/>
		/// </summary>
		private bool IsValidPhaseTransition(SessionInfo info) {
			return LastInfo.gamePhase == info.gamePhase ||
				(InStartingSequence(LastInfo.gamePhase) && LastInfo.gamePhase < info.gamePhase) ||
				(InMainPhase(LastInfo.gamePhase) && InMainPhase(info.gamePhase) && !Finished) ||
				(LastInfo.gamePhase == (int)GamePhase.Paused && info.gamePhase < (int)GamePhase.Checkered && !Finished) ||
				(LastInfo.gamePhase == (int)GamePhase.Checkered && info.gamePhase == (int)GamePhase.Paused) ||
				(LastInfo.gamePhase == (int)GamePhase.Paused && info.gamePhase == (int)GamePhase.Checkered);
		}

		private bool InStartingSequence(int phase) {
			return phase >= (int)GamePhase.Starting && phase <= (int)GamePhase.Green;
		}

		private bool InMainPhase(int phase) {
			return phase >= (int)GamePhase.Green && phase <= (int)GamePhase.Paused && phase != (int)GamePhase.Checkered;
		}

		public Session Clone() {
			Session session = Create(SessionId, LastInfo, Timestamp, Entries?.Reconstruct(), History.GetAllHistory().ConvertAll(x => x.Clone()));
			session.Update(LastInfo, LastStandings, null, LastUpdate);
			return session;
		}

		public SessionSummary Summarize(bool active) {
			int lapCount = 0;
			foreach(CarHistory car in History.GetAllHistory())
				foreach(Lap lap in car.Laps)
					if(lap != null)
						lapCount++;
			return new SessionSummary() {
				SessionId = SessionId,
				PrimaryClientId = PrimaryClientId,
				SecondaryClientIds = new List<string>(SecondaryClientIds),
				Track = Track,
				Type = Type,
				Online = Online,
				Timestamp = Timestamp,
				LastUpdate = LastUpdate,
				Finished = Finished,
				Active = active,
				CarCount = Math.Max(Math.Max(History.Count, Entries?.Slots.Count ?? 0), LastStandings?.Count ?? 0),
				LapCount = lapCount,
				Remaining = LastInfo?.timeRemainingInGamePhase ?? 0,
				Phase = LastInfo?.gamePhase ?? -1,
			};
		}
	}
}
