using LMUSessionTracker.Common.LMU;
using LMUSessionTracker.Common.Protocol;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace LMUSessionTracker.Common.Client {
	public interface ClientSessionState {
		public ProtocolState State { get; }

		public void Reset();
		public void SetState(ProtocolMessage message, ProtocolState state);
		public List<Chat> Filter(List<Chat> chats);
		public List<TeamStrategy> Filter(List<TeamStrategy> strategies);
		public StrategyUsage Filter(StrategyUsage usage);
	}

	public class DefaultClientSessionState : ClientSessionState {
		private readonly Dictionary<string, List<MultiplayerTeam>> teams = new Dictionary<string, List<MultiplayerTeam>>();
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
		}

		public void SetState(ProtocolMessage message, ProtocolState state) {
			State = state;
			if(message.MultiplayerTeams != null)
				SetTeams(message.MultiplayerTeams);
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
				Dictionary<string, List<ProtocolCarState>> carStates = AsDictionary(State.Cars);
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
						if((compound == null || compound == "N/A") || strat.lap <= carState.LastResolvedPitLap) {
							strategy.Strategy.RemoveAt(j--);
							continue;
						} else
							break;
					}
					if(strategy.Strategy.Count == 0)
						strategies.RemoveAt(i--);
				}
			}
			if(client.TraceLogging && strategies != null && strategies.Count > 0)
				logger.LogTrace($"Strategies: {System.Linq.Enumerable.Sum(strategies, x => x.Strategy == null ? 0 : x.Strategy.Count)}");
			return strategies;
		}

		private Dictionary<string, List<ProtocolCarState>> AsDictionary(List<ProtocolCarState> carStates) {
			Dictionary<string, List<ProtocolCarState>> res = new Dictionary<string, List<ProtocolCarState>>();
			foreach(ProtocolCarState carState in carStates) {
				if(!res.TryGetValue(carState.Team, out List<ProtocolCarState> possibleStates)) {
					possibleStates = new List<ProtocolCarState>();
					res.Add(carState.Team, possibleStates);
				}
				possibleStates.Add(carState);
			}
			return res;
		}

		public StrategyUsage Filter(StrategyUsage usage) {
			return usage;
		}
	}
}
