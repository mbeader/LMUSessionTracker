using LMUSessionTracker.Common.Json;
using LMUSessionTracker.Common.LMU;
using LMUSessionTracker.Common.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Websocket.Client;

namespace LMUSessionTracker.Common.Http {
	public class WebSocketLMUClient : BackgroundService {
		private static readonly string basePath = "websocket/ui";
		protected readonly ILogger<WebSocketLMUClient> logger;
		protected readonly IServiceProvider serviceProvider;
		protected readonly DateTimeProvider dateTime;
		private readonly Dictionary<string, CachedResponse> cache = new Dictionary<string, CachedResponse>() {
			{ "LiveStandings", new CachedResponse() },
			{ "SessionInfo", new CachedResponse() },
		};
		private readonly LMUClientOptions options;
		private readonly JsonSerializerOptions serializerOptions;
		private readonly string logPath;
		private string contextId = null;

		public WebSocketLMUClient(ILogger<WebSocketLMUClient> logger, IServiceProvider serviceProvider, DateTimeProvider dateTime, IOptions<LMUClientOptions> options = null) {
			this.logger = logger;
			this.serviceProvider = serviceProvider;
			this.dateTime = dateTime;
			this.options = options?.Value ?? new LMUClientOptions();
			serializerOptions = new JsonSerializerOptions(SourceGenerationContext.Default.Options) {
				TypeInfoResolver = SourceGenerationContext.Default
			};
			if(this.options.LogResponses && string.IsNullOrEmpty(this.options.LogDirectory))
				throw new Exception("Directory for response logging must be specified");
			logPath = Path.Join(Path.GetFullPath(this.options.LogDirectory), "ws");
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
			Uri uri = new Uri(new Uri(options.WebSocketBaseUri), basePath);
			var exitEvent = new ManualResetEvent(false);
			try {
				using(var client = new WebsocketClient(uri)) {
					client.ReconnectTimeout = TimeSpan.FromSeconds(30);
					client.ReconnectionHappened.Subscribe(info => {
						logger.LogDebug($"Reconnection happened, type: {info.Type}");
						if(info.Type == ReconnectionType.NoMessageReceived) {
							client.Send("{\"messageType\": \"SUB\", \"topic\": \"LiveStandings\"}");
							client.Send("{\"messageType\": \"SUB\", \"topic\": \"SessionInfo\"}");
						}
					});

					client.MessageReceived.Subscribe(ProcessMessage);
					await client.Start();
					client.Send("{\"messageType\": \"SUB\", \"topic\": \"LiveStandings\"}");
					client.Send("{\"messageType\": \"SUB\", \"topic\": \"SessionInfo\"}");

					exitEvent.WaitOne();
				}
			} catch(Exception e) {
				logger.LogError(e, $"WS failed");
			}
		}

		private void ProcessMessage(ResponseMessage message) {
			try {
				DateTime now = dateTime.UtcNow;
				contextId = now.ToString("yyyyMMddHHmmssfff");
				if(!string.IsNullOrEmpty(message.Text)) {
					WSMessage result = (WSMessage)JsonSerializer.Deserialize(message.Text, serializerOptions.GetTypeInfo(typeof(WSMessage)));
					if(result.messageType == "UPDATE" && cache.TryGetValue(result.topic, out CachedResponse cached))
						cached.Set(now, message.Text);
				}
				//LogResponse(message.Text);
			} catch(Exception e) {
				logger.LogError(e, $"Failed to process message");
			}
		}

		//protected override async Task ExecuteAsync(CancellationToken stoppingToken) {
		//	Uri uri = new Uri(new Uri(options.WebSocketBaseUri), basePath);
		//	try {
		//		using ClientWebSocket ws = new();
		//		await ws.ConnectAsync(uri, stoppingToken);
		//		ArraySegment<byte> chars = new ArraySegment<byte>(Encoding.UTF8.GetBytes("{\"messageType\": \"SUB\", \"topic\": \"LiveStandings\"}"));
		//		await ws.SendAsync(chars, WebSocketMessageType.Text, false, stoppingToken);
		//		while(!stoppingToken.IsCancellationRequested) {
		//			DateTime now = dateTime.UtcNow;
		//			contextId = now.ToString("yyyyMMddHHmmssfff");
		//			var bytes = new byte[1024];
		//			string res = "";
		//			while(true) {
		//				var result = await ws.ReceiveAsync(bytes, stoppingToken);
		//				res += Encoding.UTF8.GetString(bytes, 0, result.Count);
		//				if(result.EndOfMessage)
		//					break;
		//			}
		//			LogResponse(res);
		//		}
		//		await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client closed", default);
		//	} catch(Exception e) {
		//		logger.LogError(e, $"WS failed");
		//	}
		//}

		private void LogResponse(string json) {
			if(!options.LogResponses)
				return;
			try {
				if(!Directory.Exists(logPath))
					Directory.CreateDirectory(logPath);
				File.WriteAllText(Path.Join(logPath, $"{contextId}-raw.json"), json);
			} catch(Exception e) {
				logger.LogWarning(e, "Failed to write request debug file");
			}
		}

		private Task<string> Get(DateTime now, string topic) {
			try {
				if(cache.TryGetValue(topic, out CachedResponse value))
					return Task.FromResult(value.Get(now));
				return Task.FromResult<string>(null);
			} catch(Exception e) {
				logger.LogError(e, $"Request failed for topic: {topic}");
				return Task.FromResult<string>(null);
			}
		}

		public Task<string> GetLiveStandings(DateTime now) {
			return Get(now, "LiveStandings");
		}

		public Task<string> GetSessionInfo(DateTime now) {
			return Get(now, "SessionInfo");
		}

		private class CachedResponse {
			private static readonly int maxDiff = 200;
			private readonly object lockObj = new object();
			private string body;
			private DateTime? timestamp;

			public void Set(DateTime timestamp, string body) {
				lock(lockObj) {
					if(timestamp <= this.timestamp)
						return;
					this.timestamp = timestamp;
					this.body = body;
				}
			}

			public string Get(DateTime now) {
				lock(lockObj) {
					if(timestamp.HasValue && Math.Abs((now - timestamp.Value).TotalMilliseconds) <= maxDiff)
						return body;
					return null;
				}
			}
		}
	}
}
