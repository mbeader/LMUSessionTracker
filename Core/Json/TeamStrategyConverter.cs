using LMUSessionTracker.Core.LMU;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LMUSessionTracker.Core.Json {
	public class TeamStrategyConverter : JsonConverter<TeamStrategy> {
		public override TeamStrategy Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
			TeamStrategy res = new TeamStrategy() { Strategy = new List<Strategy>() };
			if(reader.TokenType == JsonTokenType.StartArray) {
				reader.Read();
				res.Name = reader.GetString();
				reader.Read();
				if(reader.TokenType == JsonTokenType.StartArray) {
					reader.Read();
					while(reader.TokenType != JsonTokenType.EndArray) {
						if(reader.TokenType == JsonTokenType.StartObject) {
							Strategy strategy = (Strategy)JsonDocument.ParseValue(ref reader).Deserialize(options.GetTypeInfo(typeof(Strategy)));
							if(strategy == null)
								throw new Exception("Failed to deserialize strategy");
							res.Strategy.Add(strategy);
						}
						reader.Read();
					}
					reader.Read();
				}
			}
			return res;
		}

		public override void Write(Utf8JsonWriter writer, TeamStrategy value, JsonSerializerOptions options) {
			writer.WriteStartArray();
			writer.WriteStringValue(value.Name);
			writer.WriteStartArray();
			JsonSerializer.Serialize(writer, value, options.GetTypeInfo(typeof(TeamStrategy)));
			writer.WriteEndArray();
			writer.WriteEndArray();
		}
	}
}
