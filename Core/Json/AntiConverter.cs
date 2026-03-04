using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LMUSessionTracker.Core.Json {
	public class AntiConverter<T> : JsonConverter<T> {
		public bool ThrowOnRead { get; init; }
		public bool ThrowOnWrite { get; init; }

		public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
			if(ThrowOnRead)
				throw new Exception($"Conversion of type from JSON is disallowed: {typeof(T).FullName}");
			return (T)JsonDocument.ParseValue(ref reader).Deserialize(options.GetTypeInfo(typeof(T)));
		}

		public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
			if(ThrowOnWrite)
				throw new Exception($"Conversion of type to JSON is disallowed: {typeof(T).FullName}");
			JsonSerializer.Serialize(writer, value, options.GetTypeInfo(typeof(T)));
		}
	}
}
