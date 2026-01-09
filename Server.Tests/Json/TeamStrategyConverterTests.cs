using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Server.Json;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LMUSessionTracker.Server.Tests.Json {
	public class TeamStrategyConverterTests {
		private readonly JsonSerializerSettings settings;

		public TeamStrategyConverterTests() {
			settings = new JsonSerializerSettings();
			settings.Converters.Add(new TeamStrategyNewtonsoftConverter());
		}
		[Fact]
		public void ReadJson_Sample_CanBeDeserialized() {
			List<TeamStrategy> ac = JsonConvert.DeserializeObject<List<TeamStrategy>>(json, settings);
			TeamStrategy ex = new TeamStrategy() {
				Name = "DKR Engineering",
				Strategy = new List<Strategy>() {
					new Strategy() {
						driver = "Laurents Hörr",
						driverSwap = false,
						lap = 0,
						penalty = false,
						previousStintDuration = 0.0,
						time = 0.0,
						tyres = new StrategyTires() {
							fl = new StrategyTire() {
								changed = false,
								compound = "Medium",
								New = true
							},
							fr = new StrategyTire() {
								changed = false,
								compound = "Medium",
								New = true
							},
							rl = new StrategyTire() {
								changed = false,
								compound = "Medium",
								New = true
							},
							rr = new StrategyTire() {
								changed = false,
								compound = "Medium",
								New = true
							}
						},
						ve = 0.0
					}
				}
			};
			Assert.Equal(43, ac.Count);
			Assert.Equivalent(ex, ac[0]);
		}

		[Fact]
		public void WriteJson_Sample_CanBeSerialized() {
			List<TeamStrategy> ex = JsonConvert.DeserializeObject<List<TeamStrategy>>(json, settings);
			string serialized = JsonConvert.SerializeObject(ex, settings);
			List<TeamStrategy> ac = JsonConvert.DeserializeObject<List<TeamStrategy>>(json, settings);
			Assert.Equivalent(ex, ac);
		}

		private readonly string json = """
			[["DKR Engineering", [{
					"driver": "Laurents Hörr",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Laurents Hörr",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 263.74,
					"time": 13.6171875,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["Iron Lynx - Proton", [{
					"driver": "Matteo Cairoli",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Matteo Cairoli",
					"driverSwap": false,
					"lap": 2,
					"penalty": false,
					"previousStintDuration": 270.76,
					"time": 13.296875,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["Vector Sport", [{
					"driver": "Ryan Cullen",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Ryan Cullen",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 263.8,
					"time": 13.359375,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["IDEC Sport", [{
					"driver": "Jamie Chadwick",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Jamie Chadwick",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 268.7,
					"time": 13.28125,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["Algarve Pro Racing", [{
					"driver": "Olli Caldwell",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Olli Caldwell",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 269.78000000000003,
					"time": 13.4375,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["United Autosports", [{
					"driver": "Oliver Jarvis",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Oliver Jarvis",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 269.36,
					"time": 13.1796875,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["United Autosports", [{
					"driver": "Ben Hanley",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Ben Hanley",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 267.02,
					"time": 13.3828125,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["Nielsen Racing", [{
					"driver": "Filipe Albuquerque",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Filipe Albuquerque",
					"driverSwap": false,
					"lap": 2,
					"penalty": false,
					"previousStintDuration": 274.72,
					"time": 13.1796875,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["Algarve Pro Racing", [{
					"driver": "Lorenzo Fluxá",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Lorenzo Fluxá",
					"driverSwap": false,
					"lap": 2,
					"penalty": false,
					"previousStintDuration": 289.18,
					"time": 13.3203125,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["Nielsen Racing", [{
					"driver": "James Allen",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "James Allen",
					"driverSwap": false,
					"lap": 2,
					"penalty": false,
					"previousStintDuration": 279.16,
					"time": 13.0390625,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["IDEC Sport", [{
					"driver": "Paul-Loup Chatin",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Paul-Loup Chatin",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 248.16,
					"time": 13.2578125,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["TDS Racing", [{
					"driver": "Mathias Beche",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Mathias Beche",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 260.22,
					"time": 13.15625,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["Duqueine Team", [{
					"driver": "Reshad de Gerus",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Reshad de Gerus",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 269.74,
					"time": 13.5,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["Inter Europol Competition", [{
					"driver": "Luca Ghiotto",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Luca Ghiotto",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 303.38,
					"time": 13.0625,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["CLX - Pure Rxcing", [{
					"driver": "Tom Blomqvist",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Tom Blomqvist",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 256.44,
					"time": 13.28125,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["Inter Europol Competition", [{
					"driver": "Tom Dillmann",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Tom Dillmann",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 269.84000000000003,
					"time": 13.65625,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["CLX Motorsport", [{
					"driver": "Pipo Derani",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Pipo Derani",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 251.44,
					"time": 13.578125,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["VDS Panis Racing", [{
					"driver": "Oliver Gray",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Oliver Gray",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 265.3,
					"time": 13.796875,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["Proton Competition", [{
					"driver": "René Binder",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "René Binder",
					"driverSwap": false,
					"lap": 2,
					"penalty": false,
					"previousStintDuration": 286.96,
					"time": 13.21875,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["AF Corse", [{
					"driver": "François Perrodo",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "François Perrodo",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 273.54,
					"time": 13.3984375,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["AO by TF", [{
					"driver": "Dane Cameron",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Dane Cameron",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 259.6,
					"time": 13.0390625,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["United Autosports", [{
					"driver": "Michael Birch",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.364705890417099
				}, {
					"driver": "Michael Birch",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 302.92,
					"time": 13.203125,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.3294117748737335
				}
			]], ["Richard Mille AF Corse", [{
					"driver": "Riccardo Agostini",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.37254902720451355
				}, {
					"driver": "Riccardo Agostini",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 312.74,
					"time": 13.296875,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.34117648005485535
				}
			]], ["AF Corse", [{
					"driver": "Conrad Laursen",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.37254902720451355
				}, {
					"driver": "Conrad Laursen",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 303.12,
					"time": 13.2421875,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.34117648005485535
				}
			]], ["Spirit of Race", [{
					"driver": "Duncan Cameron",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.37254902720451355
				}, {
					"driver": "Duncan Cameron",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 300.04,
					"time": 13.5625,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.34117648005485535
				}
			]], ["Kessel Racing", [{
					"driver": "Takeshi Kimura",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.37254902720451355
				}, {
					"driver": "Takeshi Kimura",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 306.68,
					"time": 13.1796875,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.34117648005485535
				}
			]], ["Racing Spirit of Léman", [{
					"driver": "Erwan Bastard",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.364705890417099
				}, {
					"driver": "Erwan Bastard",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 288.24,
					"time": 13.0390625,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.3294117748737335
				}
			]], ["Proton Competition", [{
					"driver": "Matteo Cressoni",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.364705890417099
				}, {
					"driver": "Matteo Cressoni",
					"driverSwap": false,
					"lap": 2,
					"penalty": false,
					"previousStintDuration": 317.78000000000003,
					"time": 13.296875,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.3019607961177826
				}
			]], ["Iron Lynx", [{
					"driver": "Martin Berry",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.3607843220233917
				}, {
					"driver": "Martin Berry",
					"driverSwap": false,
					"lap": 2,
					"penalty": false,
					"previousStintDuration": 311.64,
					"time": 13.359375,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.2980392277240753
				}
			]], ["JMW Motorsport", [{
					"driver": "Gianmaria Bruni",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.37254902720451355
				}, {
					"driver": "Gianmaria Bruni",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 281.8,
					"time": 15.7421875,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.34117648005485535
				}
			]], ["Kessel Racing", [{
					"driver": "Andrew Gilbert",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.37254902720451355
				}, {
					"driver": "Andrew Gilbert",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 302.68,
					"time": 13.296875,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.34117648005485535
				}
			]], ["TF Sport", [{
					"driver": "Rui Andrade",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.364705890417099
				}, {
					"driver": "Rui Andrade",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 284.22,
					"time": 13.0390625,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.3294117748737335
				}
			]], ["Iron Dames", [{
					"driver": "Sarah Bovy",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.364705890417099
				}, {
					"driver": "Sarah Bovy",
					"driverSwap": false,
					"lap": 2,
					"penalty": false,
					"previousStintDuration": 301.0,
					"time": 13.1171875,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.2980392277240753
				}
			]], ["GR Racing", [{
					"driver": "Tom Fleming",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.37254902720451355
				}, {
					"driver": "Tom Fleming",
					"driverSwap": false,
					"lap": 2,
					"penalty": false,
					"previousStintDuration": 318.58,
					"time": 13.34375,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.30980393290519714
				}
			]], ["DKR Engineering", [{
					"driver": "Wyatt Brichacek",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Wyatt Brichacek",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 278.74,
					"time": 13.2421875,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["Team Virage", [{
					"driver": "Rik Koen",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Rik Koen",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 289.2,
					"time": 13.1015625,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["EuroInternational", [{
					"driver": "Ian Aguilera",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Ian Aguilera",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 265.6,
					"time": 13.640625,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["RLR MSport", [{
					"driver": "Nick Adcock",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Nick Adcock",
					"driverSwap": false,
					"lap": 2,
					"penalty": false,
					"previousStintDuration": 289.28000000000003,
					"time": 13.2578125,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["CLX Motorsport", [{
					"driver": "Adrien Closmenil",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Adrien Closmenil",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 266.58,
					"time": 14.078125,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["Racing Spirit of Léman", [{
					"driver": "Marius Fossard",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Marius Fossard",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 272.24,
					"time": 13.296875,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["Ultimate", [{
					"driver": "Jean-Baptiste Lahaye",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Jean-Baptiste Lahaye",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 274.6,
					"time": 13.4609375,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["M Racing", [{
					"driver": "Quentin Antonel",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Quentin Antonel",
					"driverSwap": false,
					"lap": 1,
					"penalty": false,
					"previousStintDuration": 280.48,
					"time": 13.5390625,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]], ["Inter Europol Competition", [{
					"driver": "Tim Creswick",
					"driverSwap": false,
					"lap": 0,
					"penalty": false,
					"previousStintDuration": 0.0,
					"time": 0.0,
					"tyres": {
						"fl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"fr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rl": {
							"changed": false,
							"compound": "Medium",
							"new": true
						},
						"rr": {
							"changed": false,
							"compound": "Medium",
							"new": true
						}
					},
					"ve": 0.0
				}, {
					"driver": "Tim Creswick",
					"driverSwap": false,
					"lap": 2,
					"penalty": false,
					"previousStintDuration": 297.24,
					"time": 13.6171875,
					"tyres": {
						"fl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"fr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rl": {
							"changed": true,
							"compound": "Wet",
							"new": true
						},
						"rr": {
							"changed": true,
							"compound": "Wet",
							"new": true
						}
					},
					"ve": 0.0
				}
			]]]
			""";
	}
}
