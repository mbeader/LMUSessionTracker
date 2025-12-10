using LMUSessionTracker.Core.LMU;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Core.Json {
	public class TeamStrategyConverter : JsonConverter<TeamStrategy> {
		public override TeamStrategy ReadJson(JsonReader reader, Type objectType, TeamStrategy existingValue, bool hasExistingValue, JsonSerializer serializer) {
			TeamStrategy res = new TeamStrategy() { Strategy = new List<Strategy>() };
			JToken token = JToken.ReadFrom(reader);
			if(token is JArray rootarray) {
				if(rootarray.Count > 0 && rootarray[0].Type == JTokenType.String) {
					res.Name = rootarray[0].Value<string>();
				}
				if(rootarray.Count > 1 && rootarray[1].Type == JTokenType.Array) {
					JArray teamarray = rootarray[1].Value<JArray>();
					for(int i = 0; i < teamarray.Count; i++) {
						Strategy strategy = ((JObject)teamarray[i]).ToObject<Strategy>();
						if(strategy == null)
							throw new Exception("Failed to deserialize strategy");
						res.Strategy.Add(strategy);
					}
				}
			}
			return res;
		}

		public override void WriteJson(JsonWriter writer, TeamStrategy value, JsonSerializer serializer) {
			JArray teamarray = new JArray();
			foreach(Strategy strategy in value.Strategy) {
				teamarray.Add(JObject.FromObject(strategy));
			}
			JArray rootarray = new JArray() { value.Name, teamarray };
			writer.WriteToken(rootarray.CreateReader());
		}
	}
}
