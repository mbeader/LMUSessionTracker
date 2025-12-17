using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Tracking;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tests.Tracking {
	public class SessionTests {
		private static readonly string id1 = "00000000000000000000000000000001";
		private static readonly string id2 = "00000000000000000000000000000002";
		private static readonly DateTime baseTimestamp = new DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);

		public SessionTests() {
		}

		[Fact]
		public void IsSameSession_OfflineIdentical_ReturnsTrue() {
			SessionInfo info = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			Session session = Session.Create(id1, info);
			Assert.True(session.IsSameSession(info));
		}

		[Fact]
		public void IsSameSession_OfflineHigherCompletion_ReturnsTrue() {
			Session session = Session.Create(id1, new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } });
			Assert.True(session.IsSameSession(new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.6 } }));
		}

		[Fact]
		public void IsSameSession_OfflineNullOrDefault_ReturnsTrue() {
			SessionInfo infoWithValue = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			SessionInfo infoWithNull = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			SessionInfo infoWithDefault = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			Assert.True(Session.Create(id1, infoWithValue).IsSameSession(infoWithNull));
			Assert.True(Session.Create(id1, infoWithValue).IsSameSession(infoWithDefault));
			Assert.True(Session.Create(id1, infoWithNull).IsSameSession(infoWithValue));
			Assert.True(Session.Create(id1, infoWithNull).IsSameSession(infoWithNull));
			Assert.True(Session.Create(id1, infoWithNull).IsSameSession(infoWithDefault));
			Assert.True(Session.Create(id1, infoWithDefault).IsSameSession(infoWithValue));
			Assert.True(Session.Create(id1, infoWithDefault).IsSameSession(infoWithNull));
			Assert.True(Session.Create(id1, infoWithDefault).IsSameSession(infoWithDefault));
		}

		[Fact]
		public void IsSameSession_OfflineDifferentTrack_ReturnsFalse() {
			Session session = Session.Create(id1, new() { trackName = "Sebring", session = "RACE1" });
			Assert.False(session.IsSameSession(new() { trackName = "Fuji", session = "RACE1" }));
		}

		[Fact]
		public void IsSameSession_OfflineDifferentType_ReturnsFalse() {
			Session session = Session.Create(id1, new() { trackName = "Sebring", session = "QUALIFY1" });
			Assert.False(session.IsSameSession(new() { trackName = "Sebring", session = "RACE1" }));
		}

		[Fact]
		public void IsSameSession_OfflineLowerCompletion_ReturnsFalse() {
			Session session = Session.Create(id1, new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } });
			Assert.False(session.IsSameSession(new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.4 } }));
		}

		[Fact]
		public void IsSameSession_OnlineIdentical_ReturnsTrue() {
			SessionInfo info = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			MultiplayerTeams teams = new() { teams = new() { { "utid0", new() { name = "team1", drivers = new() { { "driver1", new() { roles = new() { "Driver" } } } } } } } };
			Session session = Session.Create(id1, info, teams);
			Assert.True(session.IsSameSession(info, teams));
		}

		[Fact]
		public void IsSameSession_OnlineDifferentEntryList_ReturnsFalse() {
			SessionInfo info = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			MultiplayerTeams teams = new() { teams = new() { { "utid0", new() { name = "team1", drivers = new() { { "driver1", new() { roles = new() { "Driver" } } } } } } } };
			Session session = Session.Create(id1, info, teams);
			teams.teams["utid0"].name = "team2";
			Assert.False(session.IsSameSession(info, teams));
		}

		[Fact]
		public void IsSameSession_OnlineToOffline_ReturnsFalse() {
			SessionInfo info = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			MultiplayerTeams teams = new() { teams = new() { { "utid0", new() { name = "team1", drivers = new() { { "driver1", new() { roles = new() { "Driver" } } } } } } } };
			Session session = Session.Create(id1, info, teams);
			Assert.False(session.IsSameSession(info));
		}

		[Fact]
		public void IsSameSession_OfflineToOnline_ReturnsFalse() {
			SessionInfo info = new() { trackName = "Sebring", session = "RACE1", raceCompletion = new() { timeCompletion = 0.5 } };
			MultiplayerTeams teams = new() { teams = new() { { "utid0", new() { name = "team1", drivers = new() { { "driver1", new() { roles = new() { "Driver" } } } } } } } };
			Session session = Session.Create(id1, info);
			Assert.False(session.IsSameSession(info, teams));
		}

		private SessionInfo FullSessionInfo() {
			return new SessionInfo() {
				ambientTemp = 1,
				averagePathWetness = 2,
				currentEventTime = 3,
				darkCloud = 4,
				endEventTime = 5,
				gameMode = "a",
				gamePhase = 6,
				inRealtime = true,
				lapDistance = 7,
				maxPathWetness = 8,
				maxPlayers = 9,
				maxTime = 10,
				maximumLaps = 11,
				minPathWetness = 12,
				numRedLights = 13,
				numberOfPlayers = 14,
				numberOfVehicles = 15,
				passwordProtected = false,
				playerFileName = "b",
				playerName = "c",
				raceCompletion = new Completion() { timeCompletion = 16 },
				raining = 17,
				sectorFlag = new List<string>() { "UNKNOWN", "YELLOW", "RED" },
				serverName = "d",
				serverPort = 18,
				session = "RACE1",
				startEventTime = 19,
				startLightFrame = 20,
				timeRemainingInGamePhase = 21,
				trackName = "e",
				trackTemp = 22,
				windSpeed = new Velocity() { velocity = 23, x = 24, y = 25, z = 26 },
				yellowFlagState = "f",
			};
		}

		private MultiplayerTeams FullMultiplayerTeams() {
			return new MultiplayerTeams() {
				coherenceId = 1,
				drivers = new Dictionary<string, MultiplayerDriver>() {
					{ "driver1", new MultiplayerDriver() {
						badge = "sr-saint",
						nationality = "US",
						roles = new List<string>() { "Driver" },
						teamId = "id1",
						teamName = "team1",
						uniqueTeamId = "utid0"
					} },
					{ "driver2", new MultiplayerDriver() {
						badge = "sr-noob",
						nationality = "",
						roles = new List<string>() { "Driver", "Race Engineer" },
						teamId = "id2",
						teamName = "team2",
						uniqueTeamId = "utid1"
					} }
				},
				teams = new Dictionary<string, MultiplayerTeam>() {
					{ "utid0", new MultiplayerTeam() {
						name = "team1",
						Id = "id1",
						carNumber = "1",
						vehicle = "veh1",
						drivers = new Dictionary<string, MultiplayerTeamMember>() {
							{ "driver1", new MultiplayerTeamMember() {
								badge = "sr-saint",
								nationality = "US",
								roles = new List<string>() { "Driver" }
							} },
							{ "driver3", new MultiplayerTeamMember() {
								badge = "sr-clean",
								nationality = "AU",
								roles = new List<string>() { "Race Engineer" }
							} }
						}
					} },
					{ "utid1", new MultiplayerTeam() {
						name = "team2",
						Id = "id2",
						carNumber = "2",
						vehicle = "veh2",
						drivers = new Dictionary<string, MultiplayerTeamMember>() {
							{ "driver2", new MultiplayerTeamMember() {
								badge = "sr-noob",
								nationality = "",
								roles = new List<string>() { "Driver", "Race Engineer" }
							} }
						}
					} }
				}
			};
		}

		private Lap FullLap(int index, int num) {
			return new Lap() {
				LapNumber = num,
				TotalTime = index * num * 6,
				Sector1 = index * num,
				Sector2 = index * num * 2,
				Sector3 = index * num * 3,
				Driver = $"driver{index}",
				Position = index,
				Pit = false,
				Fuel = 10 * num + index,
				VirtualEnergy = 11 * num + index,
				LFTire = 12 * num + index,
				RFTire = 13 * num + index,
				LRTire = 14 * num + index,
				RRTire = 15 * num + index,
				FinishStatus = $"{16 * num + index}",
				Timestamp = baseTimestamp + new TimeSpan(0, 0, 17 * num + index),
			};
		}

		private List<CarHistory> FullCarHistories() {
			return new List<CarHistory>() {
				new CarHistory(new CarKey() { SlotId = 0, Veh = "veh1" }, new Car() {
					SlotId = 0, Id = "id1", Veh = "veh1", TeamName = "team1", Number = "1", Class = "Hyper", VehicleName = "car1"
				}, new List<Lap>() {
					FullLap(1, 1), FullLap(1, 2)
				}),
				new CarHistory(new CarKey() { SlotId = 1, Veh = "veh2" }, new Car() {
					SlotId = 1, Id = "id2", Veh = "veh2", TeamName = "team2", Number = "2", Class = "GT3", VehicleName = "car2"
				}, new List<Lap>() {
					FullLap(2, 1)
				})
			};
		}

		private EntryList FullEntryList() {
			return new EntryList(FullMultiplayerTeams());
		}

		private List<(Car car, Lap lap)> FullLaps() {
			return new List<(Car car, Lap lap)>() {
				(new Car() {
					SlotId = 0, Id = "id1", Veh = "veh1", TeamName = "team1", Number = "1", Class = "Hyper", VehicleName = "car1"
				}, FullLap(1, 1)),
				(new Car() {
					SlotId = 0, Id = "id1", Veh = "veh1", TeamName = "team1", Number = "1", Class = "Hyper", VehicleName = "car1"
				}, FullLap(1, 2)),
				(new Car() {
					SlotId = 1, Id = "id2", Veh = "veh2", TeamName = "team2", Number = "2", Class = "GT3", VehicleName = "car2"
				}, FullLap(2, 1))
			};
		}

		[Fact]
		public void Create_SameSourceDataOnline_ReturnsIdenticalSession() {
			Session s1 = Session.Create(id1, FullSessionInfo(), FullMultiplayerTeams(), FullCarHistories());
			Session s2 = Session.Create(id1, FullSessionInfo(), FullMultiplayerTeams(), FullCarHistories());
			Assert.Equivalent(s1, s2);
			Assert.Equivalent(s1.History.GetAllHistory(), s1.History.GetAllHistory());
		}

		[Fact]
		public void Create_ReloadDataOnline_ReturnsIdenticalSession() {
			Session s1 = Session.Create(id1, FullSessionInfo(), FullMultiplayerTeams(), FullCarHistories());
			Session s2 = Session.Create(id1, FullSessionInfo(), FullEntryList(), FullLaps());
			Assert.Equivalent(s1, s2);
			Assert.Equivalent(s1.History.GetAllHistory(), s1.History.GetAllHistory());
		}
	}
}
