using LMUSessionTracker.Common.Client;
using LMUSessionTracker.Common.Http;
using LMUSessionTracker.Common.LMU;
using LMUSessionTracker.Common.Protocol;
using LMUSessionTracker.Common.Replay;
using LMUSessionTracker.Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LMUSessionTracker.Common {
	public static class HostingExtensions {
		public static void ConfigureClient(this IServiceCollection services, IConfigurationSection clientConfig) {
			services.Configure<ClientOptions>(clientConfig.GetSection("Options"));
			services.Configure<LMUClientOptions>(clientConfig.GetSection("LMU"));
			services.Configure<ProtocolClientOptions>(clientConfig.GetSection("Protocol"));
			services.Configure<ReplayOptions>(clientConfig.GetSection("Replay"));
		}

		public static IServiceCollection AddClient(this IServiceCollection services, ClientInfo clientInfo, ClientOptions options) {
			services.AddSingleton<ClientInfo>(clientInfo);
			services.AddSingleton<ProtocolSigningKey>(new ProtocolSigningKey() { Key = clientInfo.ClientId.PrivateKey });
			if(options.UseReplay) {
				services.AddScoped<ReplayLMUClient>();
				if(options.SendReplay) {
					services.AddScoped<LMUClient>(provider => provider.GetRequiredService<ReplayLMUClient>());
					services.AddScoped<ProtocolClient, HttpProtocolClient>();
					services.AddScoped<ClientHandlerFactory, DefaultClientHandlerFactory>();
					services.AddSingleton<ClientIntervalProvider, ReplayClientIntervalProvider>();
					services.AddSingleton<IntervalProvider>(provider => provider.GetRequiredService<ClientIntervalProvider>());
					services.AddSingleton<SimpleContinueProvider<ClientService>>();
					services.AddSingleton<ContinueProvider<ClientService>>(provider => provider.GetRequiredService<SimpleContinueProvider<ClientService>>());
					services.AddSingleton<ContinueProviderSource>(provider => provider.GetRequiredService<SimpleContinueProvider<ClientService>>());
					services.AddHostedService<ClientService>();
				} else
					services.AddHostedService<ReplayClientService>();
			} else {
				services.AddScoped<LMUClient, HttpLMUClient>();
				services.AddScoped<ProtocolClient, HttpProtocolClient>();
				services.AddScoped<ClientHandlerFactory, DefaultClientHandlerFactory>();
				services.AddSingleton<ClientIntervalProvider, DefaultClientIntervalProvider>();
				if(options.LMULoggingOnly)
					services.AddHostedService<ResponseLoggerService>();
				else {
					services.AddSingleton<IntervalProvider>(provider => provider.GetRequiredService<ClientIntervalProvider>());
					services.AddHostedService<ClientService>();
				}
			}
			return services;
		}
	}
}
