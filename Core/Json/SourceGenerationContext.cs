using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Protocol;
using LMUSessionTracker.Core.Replay;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

/*
 * This file contains attributes for (almost) every type that may be (de)serialized using System.Text.Json
 * Custom types always must be included, primitives are included by default
 * List<T> should not need to be handled explicitly, but Dictionary<T, T> does
 */

namespace LMUSessionTracker.Core.Json {
	[JsonSourceGenerationOptions(WriteIndented = true)]
	// LMUSessionTracker.Core.LMU
	[JsonSerializable(typeof(AttackMode))]
	[JsonSerializable(typeof(Chat))]
	[JsonSerializable(typeof(Completion))]
	[JsonSerializable(typeof(GamePhase))]
	[JsonSerializable(typeof(GameState))]
	[JsonSerializable(typeof(LMUClient))]
	[JsonSerializable(typeof(MultiplayerDriver))]
	[JsonSerializable(typeof(MultiplayerTeam))]
	[JsonSerializable(typeof(MultiplayerTeamMember))]
	[JsonSerializable(typeof(MultiplayerTeams))]
	[JsonSerializable(typeof(Position))]
	[JsonSerializable(typeof(ProfileInfo))]
	[JsonSerializable(typeof(ScheduledSession))]
	[JsonSerializable(typeof(SessionInfo))]
	[JsonSerializable(typeof(SessionsInfoForEvent))]
	[JsonSerializable(typeof(Standing))]
	[JsonSerializable(typeof(StandingsHistory))]
	[JsonSerializable(typeof(StandingsHistoryLap))]
	[JsonSerializable(typeof(Strategy))]
	[JsonSerializable(typeof(StrategyTire))]
	[JsonSerializable(typeof(StrategyTires))]
	[JsonSerializable(typeof(StrategyUsage))]
	[JsonSerializable(typeof(TeamStrategy))]
	[JsonSerializable(typeof(TrackMapPoint))]
	[JsonSerializable(typeof(Velocity))]
	[JsonSerializable(typeof(WeatherNode))]
	// LMUSessionTracker.Core.Protocol
	[JsonSerializable(typeof(ProtocolCredential))]
	[JsonSerializable(typeof(ProtocolMessage))]
	[JsonSerializable(typeof(ProtocolMessageType))]
	[JsonSerializable(typeof(ProtocolRole))]
	[JsonSerializable(typeof(ProtocolStatus))]
	// LMUSessionTracker.Core.Replay
	[JsonSerializable(typeof(StringRecord))]
	[JsonSerializable(typeof(OrderedStringRecord))]
	[JsonSerializable(typeof(IntRecord))]
	[JsonSerializable(typeof(DiscreteIntRecord))]
	[JsonSerializable(typeof(UIntRecord))]
	[JsonSerializable(typeof(DiscreteUIntRecord))]
	[JsonSerializable(typeof(BoolRecord))]
	[JsonSerializable(typeof(OrderedBoolRecord))]
	[JsonSerializable(typeof(LongRecord))]
	[JsonSerializable(typeof(DoubleRecord))]
	// external
	[JsonSerializable(typeof(List<string>))]
	[JsonSerializable(typeof(Dictionary<string, string>))]
	[JsonSerializable(typeof(Dictionary<int, int>))]
	[JsonSerializable(typeof(Dictionary<int?, int>))]
	[JsonSerializable(typeof(Dictionary<uint, int>))]
	[JsonSerializable(typeof(Dictionary<uint?, int>))]
	[JsonSerializable(typeof(Dictionary<long, int>))]
	[JsonSerializable(typeof(Dictionary<long?, int>))]
	[JsonSerializable(typeof(Dictionary<bool, int>))]
	[JsonSerializable(typeof(Dictionary<bool?, int>))]
	[JsonSerializable(typeof(SortedDictionary<string, object>))]
	[JsonSerializable(typeof(ConcurrentDictionary<string, string>))]
	[JsonSerializable(typeof(JsonElement))]
	internal partial class SourceGenerationContext : JsonSerializerContext { }
}
