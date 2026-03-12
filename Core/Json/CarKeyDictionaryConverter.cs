using LMUSessionTracker.Core.Tracking;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LMUSessionTracker.Core.Json {
	public class CarKeyDictionaryConverter<TValue> : JsonConverter<Dictionary<CarKey, TValue>> {
		public override Dictionary<CarKey, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
			var res = new Dictionary<CarKey, TValue>();
			if(reader.TokenType == JsonTokenType.StartObject) {
				reader.Read();
				while(reader.TokenType != JsonTokenType.EndObject) {
					string key = reader.GetString();
					reader.Read();
					TValue value = (TValue)JsonDocument.ParseValue(ref reader).Deserialize(options.GetTypeInfo(typeof(TValue)));
					res.Add(new CarKey(key), value);
					reader.Read();
				}
			}
			return res;
		}

		public override void Write(Utf8JsonWriter writer, Dictionary<CarKey, TValue> value, JsonSerializerOptions options) {
			writer.WriteStartObject();
			foreach(var item in value) {
				writer.WritePropertyName(item.Key.Id());
				JsonSerializer.Serialize(writer, item.Value, options.GetTypeInfo(typeof(TValue)));
			}
			writer.WriteEndObject();
		}
	}
}
