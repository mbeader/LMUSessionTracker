using LMUSessionTracker.Core.Json;
using LMUSessionTracker.Core.Tracking;
using System.Collections.Generic;
using System.Text.Json;

namespace LMUSessionTracker.Core.Tests.Json {
	public class CarKeyDictionaryConverterTests {
		private readonly JsonSerializerOptions options;

		public CarKeyDictionaryConverterTests() {
			options = new JsonSerializerOptions();
			options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			options.Converters.Add(new CarKeyDictionaryConverter<int>());
		}

		[Fact]
		public void ReadJson_Sample_CanBeDeserialized() {
			Dictionary<CarKey, int> ac = JsonSerializer.Deserialize<Dictionary<CarKey, int>>("{\"0-veh1\":3,\"1-veh2\":4}", options);
			Dictionary<CarKey, int> ex = new Dictionary<CarKey, int>() { { new CarKey(0, "veh1"), 3 }, { new CarKey(1, "veh2"), 4 } };
			Assert.Equivalent(ex, ac);
		}

		[Fact]
		public void WriteJson_Sample_CanBeSerialized() {
			Dictionary<CarKey, int> ex = JsonSerializer.Deserialize<Dictionary<CarKey, int>>("{\"0-veh1\":3,\"1-veh2\":4}", options);
			Assert.Equal("{\"0-veh1\":3,\"1-veh2\":4}", JsonSerializer.Serialize(ex, options));
		}

		[Fact]
		public void WriteJson_Sample_CanBeReserialized() {
			Dictionary<CarKey, int> ex = JsonSerializer.Deserialize<Dictionary<CarKey, int>>("{\"0-veh1\":3,\"1-veh2\":4}", options);
			string serialized = JsonSerializer.Serialize(ex, options);
			Dictionary<CarKey, int> ac = JsonSerializer.Deserialize<Dictionary<CarKey, int>>(serialized, options);
			Assert.Equivalent(ex, ac);
		}
	}
}