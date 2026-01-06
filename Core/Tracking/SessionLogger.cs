using LMUSessionTracker.Core.Json;
using LMUSessionTracker.Core.Protocol;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Tracking {
	public class SessionLogger {
		private readonly ILogger<SessionLogger> logger;
		private readonly string basePath;
		private readonly JsonSerializerOptions serializerOptions;

		public SessionLogger(ILogger<SessionLogger> logger) {
			this.logger = logger;
			basePath = Path.Join("logs", "session");
			Directory.CreateDirectory(basePath);
			serializerOptions = new JsonSerializerOptions(SourceGenerationContext.Default.Options) {
				TypeInfoResolver = SourceGenerationContext.Default,
				WriteIndented = true
			};
		}

		public async Task NewSession(string sessionId, ProtocolMessage data) {
			try {
				using FileStream stream = File.Create(Path.Join(basePath, $"{sessionId}.json"));
				await JsonSerializer.SerializeAsync(stream, data, serializerOptions.GetTypeInfo(typeof(ProtocolMessage)));
			} catch(Exception e) {
				logger.LogWarning(e, "Failed to log new session");
			}
		}
	}
}
