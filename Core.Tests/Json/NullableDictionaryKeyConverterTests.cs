using LMUSessionTracker.Core.Json;
using System.Collections.Generic;
using System.Text.Json;

namespace LMUSessionTracker.Core.Tests.Json {
	public class NullableDictionaryKeyConverter {
		private readonly JsonSerializerOptions options;

		public NullableDictionaryKeyConverter() {
			options = new JsonSerializerOptions();
			options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
			options.Converters.Add(new NullableDictionaryKeyConverter<int, int>());
		}

		[Fact]
		public void ReadJson_Sample_CanBeDeserialized() {
			Dictionary<int, int> ac = JsonSerializer.Deserialize<Dictionary<int, int>>("{\"1\":2,\"3\":4}", options);
			Dictionary<int, int> ex = new Dictionary<int, int>() { { 1, 2 }, { 3, 4 } };
			Assert.Equivalent(ex, ac);
		}

		[Fact]
		public void WriteJson_Sample_CanBeSerialized() {
			Dictionary<int, int> ex = JsonSerializer.Deserialize<Dictionary<int, int>>("{\"1\":2,\"3\":4}", options);
			string serialized = JsonSerializer.Serialize(ex, options);
			Dictionary<int, int> ac = JsonSerializer.Deserialize<Dictionary<int, int>>("{\"1\":2,\"3\":4}", options);
			Assert.Equivalent(ex, ac);
		}
	}
}