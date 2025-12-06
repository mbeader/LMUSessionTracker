using LMUSessionTracker.Core.LMU;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LMUSessionTracker.Core.Json {
	public static class SchemaValidation {
		private static readonly string filenameformat = "schema-{0}.json";
		private static readonly List<Type> types = new List<Type>() {
			typeof(Standing[]),
			typeof(AttackMode),
			typeof(Completion),
			typeof(Position),
			typeof(SessionInfo),
			typeof(Velocity),
			typeof(TrackMapPoint[]),
			typeof(Chat[]),
			typeof(MultiplayerTeams),
			typeof(StandingsHistory),
			typeof(TeamStrategy[]),
		};
		private static readonly Dictionary<string, JSchema> schemas = new Dictionary<string, JSchema>();
		private static bool loaded = false;

		public static void GenerateJsonSchema() {
			GenerateJsonSchema(types);
		}

		public static void GenerateJsonSchema(List<Type> types) {
			JSchemaGenerator generator = new JSchemaGenerator();
			generator.ContractResolver = new CamelCasePropertyNamesContractResolver();
			generator.SchemaIdGenerationHandling = SchemaIdGenerationHandling.TypeName;

			foreach(Type type in types) {
				JSchema schema = generator.Generate(type);
				File.WriteAllText(string.Format(filenameformat, type.Name), schema.ToString());
			}
		}

		public static void LoadJsonSchemas() {
			if(loaded)
				throw new Exception("Schemas already loaded");
			foreach(Type type in types) {
				string filename = Path.Join(AppContext.BaseDirectory, "Json", "Schema", string.Format(filenameformat, type.Name));
				using(StreamReader file = File.OpenText(filename))
				using(JsonTextReader reader = new JsonTextReader(file)) {
					JSchemaUrlResolver resolver = new JSchemaUrlResolver();
					JSchema schema = JSchema.Load(reader, new JSchemaReaderSettings {
						Resolver = resolver,
						BaseUri = new Uri(Path.GetFullPath(filename))
					});
					schemas.Add(type.FullName, schema);
				}
			}
			loaded = true;
		}

		public static bool Validate(string json, Type type, ILogger logger = null) {
			if(!loaded)
				throw new Exception("Schemas not loaded");
			try {
				JToken obj = JToken.Parse(json);
				JSchema schema;
				if(!schemas.TryGetValue(type.FullName, out schema)) {
					logger?.LogDebug($"Missing schema for type: {type.FullName}");
					return false;
				}
				bool res = obj.IsValid(schema, out IList<ValidationError> errors);
				if(!res) {
					logger?.LogWarning($"Validation failed for type: {type.FullName}");
					StringBuilder sb = new StringBuilder();
					for(int i = 0; i < errors.Count; i++) {
						sb.AppendLine($"Validation error #{i + 1}");
						ConcatError(sb, errors[i]);
					}
					sb.AppendLine();
					sb.AppendLine(obj.ToString());
					File.WriteAllText(Path.Join("logs", "schema", $"{DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")}-invalid-{type.Name}.txt"), sb.ToString());
				}
				return res;
			} catch(Exception e) {
				logger?.LogWarning(e, "Failed to validate JSON");
				return false;
			}
		}

		private static void ConcatError(StringBuilder sb, ValidationError error, string prefix = "") {
			sb.Append(prefix);
			sb.Append(error.Message);
			sb.AppendLine($" Path '{error.Path}', line {error.LineNumber}, position {error.LinePosition}.");
			prefix += "\t";
			foreach(ValidationError child in error.ChildErrors) {
				ConcatError(sb, child, prefix);
			}
		}
	}
}
