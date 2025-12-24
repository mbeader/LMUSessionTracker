using LMUSessionTracker.Core;
using LMUSessionTracker.Core.Client;
using LMUSessionTracker.Core.Http;
using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Protocol;
using LMUSessionTracker.Core.Replay;
using LMUSessionTracker.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LMUSessionTracker.TestClient {
	public class Orchestrator {
		private readonly Random rand = Random.Shared;
		private readonly IConfiguration configuration;
		private readonly ILoggerFactory loggerFactory;
		private readonly ILogger<Orchestrator> logger;
		private readonly List<ClientHandler> clients = new List<ClientHandler>();
		private readonly List<ReplayLMUClient> lmuClients = new List<ReplayLMUClient>();
		private ReplayLMUClient replayLMUClient;
		private ContinueProvider<Orchestrator> continueProvider;

		public Orchestrator(IConfiguration configuration, ILoggerFactory loggerFactory) {
			this.configuration = configuration;
			this.loggerFactory = loggerFactory;
			logger = loggerFactory.CreateLogger<Orchestrator>();
		}

		public void Configure() {
			var clientConfig = configuration.GetSection("Client");
			var clientOptions = clientConfig.GetSection("Options").Get<TestClientOptions>();
			for(int i = 0; i < clientOptions.NumClients; i++) {
				clients.Add(ConfigureClient(i));
			}
		}

		private ClientHandler ConfigureClient(int i) {
			var clientConfig = configuration.GetSection("Client");
			var clientOptions = clientConfig.GetSection("Options").Get<TestClientOptions>();
			string replayDirectory = !clientOptions.SingleReplay && clientOptions.ReplayDirectories.Count > i ? clientOptions.ReplayDirectories[i] : null;
			if(replayDirectory != null)
				configuration["Client:Options:Replay:Directory"] = replayDirectory;
			IOptions<LMUClientOptions> lmuClientOptions = Options.Create(clientConfig.GetSection("LMU").Get<LMUClientOptions>());
			IOptions<ProtocolClientOptions> protocolClientOptions = Options.Create(clientConfig.GetSection("Protocol").Get<ProtocolClientOptions>());
			IOptions<ReplayOptions> replayOptions = Options.Create(clientConfig.GetSection("Replay").Get<ReplayOptions>());

			ClientInfo clientInfo = new ClientInfo() {
				ClientId = ClientId.Create(false),
				OverrideInterval = clientOptions.UseReplay,
				Interval = clientConfig.GetSection("Replay")?.GetValue<int>("Interval")
			};
			logger.LogInformation($"Creating client {i} with id {clientInfo.ClientId.Hash}");
			ProtocolSigningKey signingKey = new ProtocolSigningKey() { Key = clientInfo.ClientId.PrivateKey };
			LMUClient lmuClient = null;
			if(clientOptions.UseReplay) {
				if(clientOptions.SingleReplay) {
					SimpleContinueProvider<Orchestrator> continueProvider = new SimpleContinueProvider<Orchestrator>();
					this.continueProvider = continueProvider;
					if(replayLMUClient == null)
						replayLMUClient = new ReplayLMUClient(loggerFactory.CreateLogger<ReplayLMUClient>(), replayOptions, null, continueProvider);
					lmuClient = replayLMUClient;
				} else {
					replayOptions.Value.Directory = clientOptions.ReplayDirectories[i];
					lmuClient = new ReplayLMUClient(loggerFactory.CreateLogger<ReplayLMUClient>(), replayOptions, null, null);
					lmuClients.Add((ReplayLMUClient)lmuClient);
				}
			} else {
				lmuClient = new HttpLMUClient(loggerFactory.CreateLogger<HttpLMUClient>(), null, lmuClientOptions);
			}
			HttpProtocolClient protocolClient = new HttpProtocolClient(loggerFactory.CreateLogger<HttpProtocolClient>(), signingKey, protocolClientOptions);
			return new DefaultClientHandler(lmuClient, protocolClient, clientInfo);
		}

		public async Task Run() {
			logger.LogInformation($"Starting with {clients.Count} clients");
			while(true) {
				if(!await Do())
					break;
			}
			logger.LogInformation("Stopping");
		}

		public async Task<bool> Do() {
			List<(ClientState state, ProtocolRole role, string sessionId)> lasts = new List<(ClientState state, ProtocolRole role, string sessionId)>();
			clients.ForEach(handler => lasts.Add((handler.State, handler.Role, handler.SessionId)));
			replayLMUClient?.OpenContext();
			lmuClients.ForEach(x => x.OpenContext());
			try {
				List<Task> tasks = new List<Task>();
				foreach(ClientHandler handler in clients)
					tasks.Add(Task.Delay(RandomDelay()).ContinueWith(async t => await handler.Handle()));
				await Task.WhenAll(tasks);
			} finally {
				replayLMUClient?.CloseContext();
				lmuClients.ForEach(x => x.CloseContext());
			}
			for(int i = 0; i < clients.Count; i++) {
				(ClientState state, ProtocolRole role, string sessionId) last = lasts[i];
				ClientHandler handler = clients[i];
				if(last.state != handler.State || last.role != handler.Role || last.sessionId != handler.SessionId)
					logger.LogInformation($"Client {handler.ClientId} state changed from ({last.state}, {last.role}, {last.sessionId}) to ({handler.State}, {handler.Role}, {handler.SessionId})");
			}
			return continueProvider != null ? continueProvider.ShouldContinue() : AnyClients();
		}

		private bool AnyClients() {
			for(int i = 0; i < clients.Count; i++) {
				if(lmuClients[i].Remaining == 0) {
					logger.LogInformation($"Client {clients[i].ClientId} complete");
					clients.RemoveAt(i);
					lmuClients.RemoveAt(i);
					i--;
				}
			}
			return clients.Count > 0;
		}

		private int RandomDelay() {
			double mean = 0;
			double stdDev = 2500;

			// https://stackoverflow.com/a/218600
			double u1 = 1.0 - rand.NextDouble(); //uniform(0,1] random doubles
			double u2 = 1.0 - rand.NextDouble();
			double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2); //random normal(0,1)
			double randNormal = mean + stdDev * randStdNormal; //random normal(mean,stdDev^2)

			double abs = Math.Abs(randNormal);
			return (int)(abs < 5000.0 ? abs / 100.0 : 5000.0);
		}
	}
}
