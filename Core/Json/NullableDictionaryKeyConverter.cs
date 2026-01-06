using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LMUSessionTracker.Core.Json {
	public class NullableDictionaryKeyConverter<TKey, TValue> : JsonConverter<Dictionary<TKey?, TValue>> where TKey : struct {
		public override Dictionary<TKey?, TValue> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
			var res = new Dictionary<TKey?, TValue>();
			foreach(var item in (Dictionary<TKey, TValue>)JsonDocument.ParseValue(ref reader).Deserialize(options.GetTypeInfo(typeof(Dictionary<TKey, TValue>))))
				res.Add(item.Key, item.Value);
			return res;
		}

		public override void Write(Utf8JsonWriter writer, Dictionary<TKey?, TValue> value, JsonSerializerOptions options) {
			Dictionary<TKey, TValue> nonnull = new Dictionary<TKey, TValue>();
			foreach(var item in value)
				nonnull.Add(item.Key.Value, item.Value);
			JsonSerializer.Serialize(writer, nonnull, options.GetTypeInfo(typeof(Dictionary<TKey, TValue>)));
		}
	}
}
