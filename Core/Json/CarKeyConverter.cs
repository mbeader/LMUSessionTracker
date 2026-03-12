using LMUSessionTracker.Core.Tracking;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LMUSessionTracker.Core.Json {
	public class CarKeyConverter : JsonConverter<CarKey> {
		public override CarKey Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
			string key = (string)JsonDocument.ParseValue(ref reader).Deserialize(options.GetTypeInfo(typeof(string)));
			return new CarKey(key);
		}

		public override void Write(Utf8JsonWriter writer, CarKey value, JsonSerializerOptions options) {
			JsonSerializer.Serialize(writer, value.Id(), options.GetTypeInfo(typeof(string)));
		}
	}
}
