using Json.Schema;
using LMUSessionTracker.Core.LMU;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace LMUSessionTracker.Core.Json {
	public class SystemTextJsonSchemaValidator : SchemaValidator {
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
			typeof(StrategyUsage),
			typeof(ProfileInfo),
			typeof(SessionsInfoForEvent),
			typeof(GameState),
		};
		private static readonly Dictionary<string, JsonSchema> schemas = new Dictionary<string, JsonSchema>();
		private static readonly Dictionary<string, JsonSchema> listSchemas = new Dictionary<string, JsonSchema>();
		private static readonly JsonSerializerOptions serializerOptions = new JsonSerializerOptions() { WriteIndented = true };
		private static bool loaded = false;

		// TODO: figure out why JsonSchema.Net can't actually generate schemas
		//public static void GenerateJsonSchema() {
		//	GenerateJsonSchema(types);
		//}

		//public static void GenerateJsonSchema(List<Type> types) {
		//	JsonSchemaBuilder builder = new JsonSchemaBuilder();
		//	foreach(Type type in types) {
		//		JsonSchema schema = builder.FromType(type);
		//		File.WriteAllText(string.Format(filenameformat, type.Name), schema.ToString());
		//	}
		//}

		public static void LoadJsonSchemas() {
			if(loaded)
				throw new Exception("Schemas already loaded");
			foreach(string filename in Directory.GetFiles(Path.Join(AppContext.BaseDirectory, "Json", "Schema"))) {
				JsonSchema schema = JsonSchema.FromFile(filename, baseUri: new Uri(Path.GetFullPath(filename)));
				SchemaRegistry.Global.Register(new Uri(Path.GetFullPath(filename)), schema);
			}
			foreach(Type type in types) {
				string filename = Path.Join(AppContext.BaseDirectory, "Json", "Schema", string.Format(filenameformat, type.Name));
				var basedoc = SchemaRegistry.Global.Get(new Uri(Path.GetFullPath(Path.Join(AppContext.BaseDirectory, "Json", "Schema", type.Name))));
				if(basedoc is JsonSchema schema) {
					schemas.Add(type.FullName, schema);
					if(type.BaseType == typeof(Array)) {
						listSchemas.Add(type.GetElementType().FullName, schema);
					}
				}
			}
			schemas.Add(typeof(string).FullName, JsonSchema.Build(JsonDocument.Parse("{\"type\":\"string\"}").RootElement));
			loaded = true;
		}

		public static bool Validate(string json, Type type, string id = null, ILogger logger = null) {
			if(!loaded)
				throw new Exception("Schemas not loaded");
			try {
				bool isList = type.UnderlyingSystemType?.Name == "List`1";
				JsonDocument obj = JsonDocument.Parse(json);
				JsonSchema schema;
				if(isList ? !listSchemas.TryGetValue(type.GenericTypeArguments[0].FullName, out schema) : !schemas.TryGetValue(type.FullName, out schema)) {
					logger?.LogDebug($"Missing schema for type: {type.FullName}");
					return false;
				}
				EvaluationResults result = schema.Evaluate(obj.RootElement, new EvaluationOptions() {
					AddAnnotationForUnknownKeywords = true,
					PreserveDroppedAnnotations = true,
					OutputFormat = OutputFormat.List
				});
				if(!result.IsValid) {
					logger?.LogWarning($"Validation failed for type: {type.FullName}");
					StringBuilder sb = new StringBuilder();
					int i = 0;
					i = ConcatError(sb, result, i);
					foreach(EvaluationResults subresult in result.Details) {
						i = ConcatError(sb, subresult, i);
					}
					sb.AppendLine();
					sb.AppendLine(JsonSerializer.Serialize(obj.RootElement, serializerOptions));
					string runId = id ?? DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
					File.WriteAllText(Path.Join("logs", "schema", $"{runId}-invalid-{(isList ? $"{type.GenericTypeArguments[0].Name}[]" : type.Name)}.txt"), sb.ToString());
				}
				return result.IsValid;
			} catch(Exception e) {
				logger?.LogWarning(e, "Failed to validate JSON");
				return false;
			}
		}

		private static int ConcatError(StringBuilder sb, EvaluationResults result, int i) {
			if(result.Errors != null) {
				foreach(string error in result.Errors.Keys) {
					sb.AppendLine($"Validation error #{i + 1}");
					sb.AppendLine(result.EvaluationPath.ToString());
					sb.AppendLine(result.InstanceLocation.ToString());
					sb.AppendLine($"{error}: {result.Errors[error]}");
					i++;
				}
			}
			return i;
		}

		private readonly ILogger<SystemTextJsonSchemaValidator> logger;
		private readonly SchemaValidatorOptions options;

		public SystemTextJsonSchemaValidator(ILogger<SystemTextJsonSchemaValidator> logger, IOptions<SchemaValidatorOptions> options = null) {
			this.logger = logger;
			this.options = options?.Value ?? new SchemaValidatorOptions();
		}

		public bool Validate(string json, Type type, string id = null) {
			return Validate(json, type, id, logger);
		}
	}
}
