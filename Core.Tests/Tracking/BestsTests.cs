using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Tracking;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tests.Tracking {
	public class BestsTests {
		private Bests bests;

		public BestsTests() {
			bests = new Bests();
		}

		private Lap Lap1(string name = null) => new Lap() { Driver = name ?? "driver1", Sector1 = 10, Sector2 = 20, Sector3 = 30, TotalTime = 60 };
		private Lap Lap2(string name = null) => new Lap() { Driver = name ?? "driver1", Sector1 = 11, Sector2 = 22, Sector3 = 33, TotalTime = 66 };
		private Lap Lap3(string name = null) => new Lap() { Driver = name ?? "driver1", Sector1 = 9, Sector2 = 19, Sector3 = 29, TotalTime = 57 };

		private Best Best1() => new Best() { Sector1 = 10, Sector2 = 20, Sector3 = 30, Total = 60 };
		private Best Best2() => new Best() { Sector1 = 11, Sector2 = 22, Sector3 = 33, Total = 66 };
		private Best Best3() => new Best() { Sector1 = 9, Sector2 = 19, Sector3 = 29, Total = 57 };

		[Fact]
		public void Construct_Empty_NoTimes() {
			bests = new Bests(new());
			Assert.Empty(bests.Class);
			Assert.Empty(bests.Car);
			Assert.Empty(bests.Driver);
		}

		[Fact]
		public void Construct_InvalidLap_NoTimes() {
			bests = new Bests(new() { new(new(0, "veh1"), new() { Class = "GT3", SlotId = 0, Veh = "veh1" }, new() { new Lap() { Driver = "driver1" } }) });
			Assert.Equivalent(new Dictionary<string, Best>() { { "GT3", new() } }, bests.Class);
			Assert.Equivalent(new Dictionary<CarKey, Best>() { { new(0, "veh1"), new() } }, bests.Car);
			Assert.Equivalent(new Dictionary<CarKey, Dictionary<string, Best>>() { { new(0, "veh1"), new() { { "driver1", new() } } } }, bests.Driver);
		}

		[Fact]
		public void Construct_FirstLap_OneClassCarDriverTimes() {
			bests = new Bests(new() { new(new(0, "veh1"), new() { Class = "GT3", SlotId = 0, Veh = "veh1" }, new() { Lap1() }) });
			Best ex = new Best() { Sector1 = 10, Sector2 = 20, Sector3 = 30, Total = 60 };
			Assert.Equivalent(new Dictionary<string, Best>() { { "GT3", Best1() } }, bests.Class);
			Assert.Equivalent(new Dictionary<CarKey, Best>() { { new(0, "veh1"), Best1() } }, bests.Car);
			Assert.Equivalent(new Dictionary<CarKey, Dictionary<string, Best>>() { { new(0, "veh1"), new() { { "driver1", Best1() } } } }, bests.Driver);
		}

		[Fact]
		public void Construct_SecondLapAllSlower_OneClassCarDriverTimes() {
			bests = new Bests(new() { new(new(0, "veh1"), new() { Class = "GT3", SlotId = 0, Veh = "veh1" }, new() { Lap1(), Lap2() }) });
			Assert.Equivalent(new Dictionary<string, Best>() { { "GT3", Best1() } }, bests.Class);
			Assert.Equivalent(new Dictionary<CarKey, Best>() { { new(0, "veh1"), Best1() } }, bests.Car);
			Assert.Equivalent(new Dictionary<CarKey, Dictionary<string, Best>>() { { new(0, "veh1"), new() { { "driver1", Best1() } } } }, bests.Driver);
		}

		[Fact]
		public void Construct_SecondLapAllFaster_OneClassCarDriverTimes() {
			bests = new Bests(new() { new(new(0, "veh1"), new() { Class = "GT3", SlotId = 0, Veh = "veh1" }, new() { Lap1(), Lap3() }) });
			Assert.Equivalent(new Dictionary<string, Best>() { { "GT3", Best3() } }, bests.Class);
			Assert.Equivalent(new Dictionary<CarKey, Best>() { { new(0, "veh1"), Best3() } }, bests.Car);
			Assert.Equivalent(new Dictionary<CarKey, Dictionary<string, Best>>() { { new(0, "veh1"), new() { { "driver1", Best3() } } } }, bests.Driver);
		}

		[Fact]
		public void Construct_MultipleDriversSameCar_OneClassCarTwoDriverTimes() {
			bests = new Bests(new() { new(new(0, "veh1"), new() { Class = "GT3", SlotId = 0, Veh = "veh1" }, new() { Lap1(), Lap2("driver2") }) });
			Assert.Equivalent(new Dictionary<string, Best>() { { "GT3", Best1() } }, bests.Class);
			Assert.Equivalent(new Dictionary<CarKey, Best>() { { new(0, "veh1"), Best1() } }, bests.Car);
			Assert.Equivalent(new Dictionary<CarKey, Dictionary<string, Best>>() { { new(0, "veh1"), new() { { "driver1", Best1() }, { "driver2", Best2() } } } }, bests.Driver);
		}

		[Fact]
		public void Construct_MultipleCarsSameDriver_OneClassTwoCarDriverTimes() {
			bests = new Bests(new() { new(new(0, "veh1"), new() { Class = "GT3", SlotId = 0, Veh = "veh1" }, new() { Lap1() }), new(new(1, "veh2"), new() { Class = "GT3", SlotId = 1, Veh = "veh2" }, new() { Lap2() }) });
			Assert.Equivalent(new Dictionary<string, Best>() { { "GT3", Best1() } }, bests.Class);
			Assert.Equivalent(new Dictionary<CarKey, Best>() { { new(0, "veh1"), Best1() }, { new(1, "veh2"), Best2() } }, bests.Car);
			Assert.Equivalent(new Dictionary<CarKey, Dictionary<string, Best>>() { { new(0, "veh1"), new() { { "driver1", Best1() } } }, { new(1, "veh2"), new() { { "driver1", Best2() } } } }, bests.Driver);
		}

		[Fact]
		public void Construct_MultipleClasses_TwoClassCarDriverTimes() {
			bests = new Bests(new() { new(new(0, "veh1"), new() { Class = "GT3", SlotId = 0, Veh = "veh1" }, new() { Lap1() }), new(new(1, "veh2"), new() { Class = "Hyper", SlotId = 1, Veh = "veh2" }, new() { Lap2("driver2") }) });
			Assert.Equivalent(new Dictionary<string, Best>() { { "GT3", Best1() }, { "Hyper", Best2() } }, bests.Class);
			Assert.Equivalent(new Dictionary<CarKey, Best>() { { new(0, "veh1"), Best1() }, { new(1, "veh2"), Best2() } }, bests.Car);
			Assert.Equivalent(new Dictionary<CarKey, Dictionary<string, Best>>() { { new(0, "veh1"), new() { { "driver1", Best1() } } }, { new(1, "veh2"), new() { { "driver2", Best2() } } } }, bests.Driver);
		}

		[Fact]
		public void Update_MissingClassCarAndDriver_NoTimes() {
			Assert.False(bests.Update(new(new(), new())));
			Assert.Empty(bests.Class);
			Assert.Empty(bests.Car);
			Assert.Empty(bests.Driver);
		}

		[Fact]
		public void Update_InvalidLap_NoTimes() {
			Assert.False(bests.Update(new(new() { Class = "GT3", SlotId = 0, Veh = "veh1" }, new Lap() { Driver = "driver1" })));
			Assert.Equivalent(new Dictionary<string, Best>() { { "GT3", new() } }, bests.Class);
			Assert.Equivalent(new Dictionary<CarKey, Best>() { { new(0, "veh1"), new() } }, bests.Car);
			Assert.Equivalent(new Dictionary<CarKey, Dictionary<string, Best>>() { { new(0, "veh1"), new() { { "driver1", new() } } } }, bests.Driver);
		}

		[Fact]
		public void Update_FirstLap_OneClassCarDriverTimes() {
			Assert.True(bests.Update(new(new() { Class = "GT3", SlotId = 0, Veh = "veh1" }, Lap1())));
			Best ex = new Best() { Sector1 = 10, Sector2 = 20, Sector3 = 30, Total = 60 };
			Assert.Equivalent(new Dictionary<string, Best>() { { "GT3", Best1() } }, bests.Class);
			Assert.Equivalent(new Dictionary<CarKey, Best>() { { new(0, "veh1"), Best1() } }, bests.Car);
			Assert.Equivalent(new Dictionary<CarKey, Dictionary<string, Best>>() { { new(0, "veh1"), new() { { "driver1", Best1() } } } }, bests.Driver);
		}

		[Fact]
		public void Update_SecondLapAllSlower_OneClassCarDriverTimes() {
			Assert.True(bests.Update(new(new() { Class = "GT3", SlotId = 0, Veh = "veh1" }, Lap1())));
			Assert.False(bests.Update(new(new() { Class = "GT3", SlotId = 0, Veh = "veh1" }, Lap2())));
			Assert.Equivalent(new Dictionary<string, Best>() { { "GT3", Best1() } }, bests.Class);
			Assert.Equivalent(new Dictionary<CarKey, Best>() { { new(0, "veh1"), Best1() } }, bests.Car);
			Assert.Equivalent(new Dictionary<CarKey, Dictionary<string, Best>>() { { new(0, "veh1"), new() { { "driver1", Best1() } } } }, bests.Driver);
		}

		[Fact]
		public void Update_SecondLapAllFaster_OneClassCarDriverTimes() {
			Assert.True(bests.Update(new(new() { Class = "GT3", SlotId = 0, Veh = "veh1" }, Lap1())));
			Assert.True(bests.Update(new(new() { Class = "GT3", SlotId = 0, Veh = "veh1" }, Lap3())));
			Assert.Equivalent(new Dictionary<string, Best>() { { "GT3", Best3() } }, bests.Class);
			Assert.Equivalent(new Dictionary<CarKey, Best>() { { new(0, "veh1"), Best3() } }, bests.Car);
			Assert.Equivalent(new Dictionary<CarKey, Dictionary<string, Best>>() { { new(0, "veh1"), new() { { "driver1", Best3() } } } }, bests.Driver);
		}

		[Fact]
		public void Update_MultipleDriversSameCar_OneClassCarTwoDriverTimes() {
			Assert.True(bests.Update(new(new() { Class = "GT3", SlotId = 0, Veh = "veh1" }, Lap1())));
			Assert.True(bests.Update(new(new() { Class = "GT3", SlotId = 0, Veh = "veh1" }, Lap2("driver2"))));
			Assert.Equivalent(new Dictionary<string, Best>() { { "GT3", Best1() } }, bests.Class);
			Assert.Equivalent(new Dictionary<CarKey, Best>() { { new(0, "veh1"), Best1() } }, bests.Car);
			Assert.Equivalent(new Dictionary<CarKey, Dictionary<string, Best>>() { { new(0, "veh1"), new() { { "driver1", Best1() }, { "driver2", Best2() } } } }, bests.Driver);
		}

		[Fact]
		public void Update_MultipleCarsSameDriver_OneClassTwoCarDriverTimes() {
			Assert.True(bests.Update(new(new() { Class = "GT3", SlotId = 0, Veh = "veh1" }, Lap1())));
			Assert.True(bests.Update(new(new() { Class = "GT3", SlotId = 1, Veh = "veh2" }, Lap2())));
			Assert.Equivalent(new Dictionary<string, Best>() { { "GT3", Best1() } }, bests.Class);
			Assert.Equivalent(new Dictionary<CarKey, Best>() { { new(0, "veh1"), Best1() }, { new(1, "veh2"), Best2() } }, bests.Car);
			Assert.Equivalent(new Dictionary<CarKey, Dictionary<string, Best>>() { { new(0, "veh1"), new() { { "driver1", Best1() } } }, { new(1, "veh2"), new() { { "driver1", Best2() } } } }, bests.Driver);
		}

		[Fact]
		public void Update_MultipleClasses_TwoClassCarDriverTimes() {
			Assert.True(bests.Update(new(new() { Class = "GT3", SlotId = 0, Veh = "veh1" }, Lap1())));
			Assert.True(bests.Update(new(new() { Class = "Hyper", SlotId = 1, Veh = "veh2" }, Lap2("driver2"))));
			Assert.Equivalent(new Dictionary<string, Best>() { { "GT3", Best1() }, { "Hyper", Best2() } }, bests.Class);
			Assert.Equivalent(new Dictionary<CarKey, Best>() { { new(0, "veh1"), Best1() }, { new(1, "veh2"), Best2() } }, bests.Car);
			Assert.Equivalent(new Dictionary<CarKey, Dictionary<string, Best>>() { { new(0, "veh1"), new() { { "driver1", Best1() } } }, { new(1, "veh2"), new() { { "driver2", Best2() } } } }, bests.Driver);
		}
	}
}
