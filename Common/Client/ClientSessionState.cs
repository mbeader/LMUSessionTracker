using LMUSessionTracker.Common.LMU;
using LMUSessionTracker.Common.Protocol;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Common.Client {
	public interface ClientSessionState {
		public ProtocolState State { get; }

		public void Reset();
		public void SetState(ProtocolMessage message, ProtocolState state);
		public List<Chat> Filter(List<Chat> chats);
		public List<TeamStrategy> Filter(List<TeamStrategy> strategies);
		public StrategyUsage Filter(StrategyUsage usage);
		public List<WSStandingSubset> Filter(WSMessageLiveStandings standings);
	}

	public class DefaultClientSessionState : ClientSessionState {
		private readonly Dictionary<string, List<MultiplayerTeam>> teams = new Dictionary<string, List<MultiplayerTeam>>();
		private readonly Dictionary<string, List<int>> drivers = new Dictionary<string, List<int>>();
		private readonly ILogger<DefaultClientSessionState> logger;
		private readonly ClientInfo client;

		public ProtocolState State { get; private set; }

		public DefaultClientSessionState(ILogger<DefaultClientSessionState> logger, ClientInfo client) {
			this.logger = logger;
			this.client = client;
		}

		public void Reset() {
			State = null;
			teams.Clear();
			drivers.Clear();
		}

		public void SetState(ProtocolMessage message, ProtocolState state) {
			State = state;
			if(message.MultiplayerTeams != null)
				SetTeams(message.MultiplayerTeams);
			if(message.Standings != null)
				SetDrivers(message.Standings);
		}

		private void SetTeams(MultiplayerTeams multiplayerTeams) {
			foreach((string utid, MultiplayerTeam team) in multiplayerTeams.teams) {
				if(!teams.TryGetValue(team.name, out List<MultiplayerTeam> possibleTeams)) {
					possibleTeams = new List<MultiplayerTeam>();
					teams.Add(team.name, possibleTeams);
				}
				bool anyMatch = false;
				foreach(MultiplayerTeam possibleTeam in possibleTeams) {
					bool match = true;
					foreach((string possibleName, MultiplayerTeamMember possibleMember) in possibleTeam.drivers) {
						if(!team.drivers.ContainsKey(possibleName)) {
							match = false;
							break;
						}
					}
					if(match) {
						anyMatch = true;
						break;
					}
				}
				if(!anyMatch)
					possibleTeams.Add(team);
			}
		}

		private void SetDrivers(List<Standing> standings) {
			drivers.Clear();
			foreach(Standing standing in standings) {
				if(!drivers.TryGetValue(standing.driverName, out List<int> possibleSlots)) {
					possibleSlots = new List<int>();
					drivers.Add(standing.driverName, possibleSlots);
				}
				possibleSlots.Add(standing.slotID);
			}
		}

		public List<Chat> Filter(List<Chat> chats) {
			if(chats == null || chats.Count == 0)
				return null;
			if(State == null || State.Chat == null)
				return chats;
			Chat last = chats[^1];
			if(last.timestamp == State.Chat.timestamp && last.message == State.Chat.message)
				return null;
			int startIndex = -1;
			for(int i = chats.Count - 1; i >= 0; i--) {
				if(chats[i].timestamp > State.Chat.timestamp)
					startIndex = i;
				else if(chats[i].timestamp == State.Chat.timestamp && chats[i].message != State.Chat.message)
					startIndex = i;
				else
					break;
			}
			if(startIndex < 0)
				return null;
			return chats.GetRange(startIndex, chats.Count - startIndex);
		}

		public List<TeamStrategy> Filter(List<TeamStrategy> strategies) {
			if(strategies != null && strategies.Count > 0 && State != null && State.Cars != null) {
				Dictionary<string, List<ProtocolCarState>> carStates = AsDictionary(State.Cars, carState => carState.Team);
				for(int i = 0; i < strategies.Count; i++) {
					TeamStrategy strategy = strategies[i];
					if(!carStates.TryGetValue(strategy.Name, out List<ProtocolCarState> possibleCarStates)) {
						strategies.RemoveAt(i--);
						continue;
					}

					ProtocolCarState carState = null;
					if(teams.Count > 0) {
						if(teams.TryGetValue(strategy.Name, out List<MultiplayerTeam> possibleTeams)) {
							foreach(MultiplayerTeam possibleTeam in possibleTeams) {
								if(strategy.Strategy.TrueForAll(x => possibleTeam.drivers.ContainsKey(x.driver)))
									foreach(ProtocolCarState possibleCarState in possibleCarStates) {
										if(possibleTeam.drivers.ContainsKey(possibleCarState.Driver)) {
											carState = possibleCarState;
											break;
										}
									}
								if(carState != null)
									break;
							}
						}
					} else {
						// offline handling
						foreach(ProtocolCarState possibleCarState in possibleCarStates) {
							if(strategy.Strategy.Exists(x => x.driver == possibleCarState.Driver)) {
								carState = possibleCarState;
								break;
							}
						}
					}
					if(carState == null) {
						strategies.RemoveAt(i--);
						continue;
					}

					for(int j = 0; j < strategy.Strategy.Count; j++) {
						Strategy strat = strategy.Strategy[j];
						string compound = strat.tyres?.fl?.compound;
						// disallow same lap pits except if strat is actually lap 1
						if((compound == null || compound == "N/A") || (strat.lap <= carState.LastResolvedPitLap && !(strat.lap == 1 && carState.LastResolvedPitLap == 1))) {
							strategy.Strategy.RemoveAt(j--);
							continue;
						} else
							break;
					}
					if(strategy.Strategy.Count == 0)
						strategies.RemoveAt(i--);
				}
			}
			//if(client.TraceLogging && strategies != null && strategies.Count > 0)
			//	logger.LogTrace($"Strategies: {System.Linq.Enumerable.Sum(strategies, x => x.Strategy == null ? 0 : x.Strategy.Count)}");
			return strategies;
		}

		private Dictionary<string, List<ProtocolCarState>> AsDictionary(List<ProtocolCarState> carStates, Func<ProtocolCarState, string> keyFunc) {
			Dictionary<string, List<ProtocolCarState>> res = new Dictionary<string, List<ProtocolCarState>>();
			foreach(ProtocolCarState carState in carStates) {
				string key = keyFunc(carState);
				if(key == null)
					continue;
				if(!res.TryGetValue(key, out List<ProtocolCarState> possibleStates)) {
					possibleStates = new List<ProtocolCarState>();
					res.Add(key, possibleStates);
				}
				possibleStates.Add(carState);
			}
			return res;
		}

		public StrategyUsage Filter(StrategyUsage usage) {
			if(usage != null && usage.Count > 0 && State != null && State.Cars != null) {
				Dictionary<string, List<ProtocolCarState>> carStates = AsDictionary(State.Cars, carState => carState.Driver);
				foreach((string driver, List<StrategyDriverUsage> driverUsage) in usage) {
					if(!carStates.TryGetValue(driver, out List<ProtocolCarState> possibleCarStates)) {
						usage.Remove(driver);
						continue;
					}

					List<int> possibleSlots = drivers.GetValueOrDefault(driver);
					ProtocolCarState carState = null;
					if(possibleCarStates.Count > 1) {
						if(possibleSlots != null && possibleSlots.Count == 1)
							carState = possibleCarStates.Find(x => x.SlotId == possibleSlots[0]);
						if(carState == null) {
							if(client.TraceLogging) {
								string ids = string.Join(", ", possibleCarStates.ConvertAll(x => $"{x.SlotId}-{x.Veh}"));
								if(possibleSlots != null && possibleSlots.Count > 1)
									logger.LogTrace($"Unable to find state for usage: {driver} [{ids}] {string.Join(", ", possibleSlots)}");
								else
									logger.LogTrace($"Unable to find state for usage: {driver} [{ids}]");
							}
							usage.Remove(driver);
							continue;
						}
					} else {
						carState = possibleCarStates[0];
						//if(client.TraceLogging && possibleSlots != null && !possibleSlots.Contains(possibleCarStates[0].SlotId))
						//	logger.LogTrace($"Mismatched usage slot: {driver} [{possibleCarStates[0].SlotId} not in {string.Join(", ", possibleSlots)}]");
					}

					for(int j = 0; j < driverUsage.Count; j++) {
						StrategyDriverUsage lapUsage = driverUsage[j];
						if(lapUsage.lap <= carState.LastResolvedLapLap || lapUsage.lap == 0) {
							driverUsage.RemoveAt(j--);
							continue;
						} else
							break;
					}
					if(driverUsage.Count == 0)
						usage.Remove(driver);
				}
			}
			//if(client.TraceLogging && usage != null && usage.Count > 0)
			//	logger.LogTrace($"Usages: {System.Linq.Enumerable.Sum(usage, x => x.Value == null ? 0 : x.Value.Count)}");
			return usage;
		}

		public List<WSStandingSubset> Filter(WSMessageLiveStandings standings) {
			if(standings?.body != null && standings.body.Count > 0) {
				return standings.body.ConvertAll(x => new WSStandingSubset() {
					compoundNames = x.compoundNames,
					penalties = x.penalties,
					slotID = x.slotID,
					vehFilename = x.vehFilename,
					virtualEnergy = x.virtualEnergy
				});
			}
			return null;
		}
	}
}
