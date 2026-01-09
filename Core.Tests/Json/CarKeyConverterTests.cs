using LMUSessionTracker.Core.Json;
using LMUSessionTracker.Core.Tracking;
using System.Text.Json;

namespace LMUSessionTracker.Core.Tests.Json {
	public class CarKeyConverterTests {
		private readonly JsonSerializerOptions options;

		public CarKeyConverterTests() {
			options = new JsonSerializerOptions();
			options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			options.Converters.Add(new CarKeyConverter());
		}

		[Fact]
		public void ReadJson_Sample_CanBeDeserialized() {
			CarKey ac = JsonSerializer.Deserialize<CarKey>("\"0-veh1\"", options);
			CarKey ex = new CarKey(0, "veh1");
			Assert.Equivalent(ex, ac);
		}

		[Fact]
		public void WriteJson_Sample_CanBeSerialized() {
			CarKey ex = JsonSerializer.Deserialize<CarKey>("\"0-veh1\"", options);
			Assert.Equal("\"0-veh1\"", JsonSerializer.Serialize(ex, options));
		}

		[Fact]
		public void WriteJson_Sample_CanBeReserialized() {
			CarKey ex = JsonSerializer.Deserialize<CarKey>("\"0-veh1\"", options);
			string serialized = JsonSerializer.Serialize(ex, options);
			CarKey ac = JsonSerializer.Deserialize<CarKey>(serialized, options);
			Assert.Equivalent(ex, ac);
		}
	}
}