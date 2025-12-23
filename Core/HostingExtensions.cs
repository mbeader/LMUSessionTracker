using LMUSessionTracker.Core.Client;
using LMUSessionTracker.Core.Http;
using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Protocol;
using LMUSessionTracker.Core.Replay;
using LMUSessionTracker.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LMUSessionTracker.Core {
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
					services.AddSingleton<SimpleContinueProvider<ClientService>>();
					services.AddSingleton<ContinueProvider<ClientService>>(provider => provider.GetRequiredService<SimpleContinueProvider<ClientService>>());
					services.AddSingleton<ContinueProviderSource>(provider => provider.GetRequiredService<SimpleContinueProvider<ClientService>>());
					services.AddHostedService<ClientService>();
				} else
					services.AddHostedService<ReplayClientService>();
			} else {
				services.AddScoped<LMUClient, HttpLMUClient>();
				services.AddScoped<ProtocolClient, HttpProtocolClient>();
				if(options.LMULoggingOnly)
					services.AddHostedService<ResponseLoggerService>();
				else
					services.AddHostedService<ClientService>();
			}
			return services;
		}
	}
}
