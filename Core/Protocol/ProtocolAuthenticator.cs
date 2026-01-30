using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSec.Cryptography;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Protocol {
	public interface ProtocolAuthenticator {
		public Task<bool?> Verify(HttpRequest request, ProtocolMessage data);
		public Task<string> Authenticate(HttpRequest request, ProtocolCredential credential);
	}

	public class DefaultProtocolAuthenticator : ProtocolAuthenticator {
		private static readonly Version minVersion = new Version(0, 7, 0);
		private static readonly Version maxVersion = new Version(1, 0, 0);
		private static readonly SignatureAlgorithm algorithm = SignatureAlgorithm.Ed25519;
		private readonly ConcurrentDictionary<string, PublicKey> clientKeys = new ConcurrentDictionary<string, PublicKey>();
		private readonly ILogger<DefaultProtocolAuthenticator> logger;

		public static Version MinVersion => minVersion;
		public static Version MaxVersion => maxVersion;

		public DefaultProtocolAuthenticator(ILogger<DefaultProtocolAuthenticator> logger) {
			this.logger = logger;
		}

		public async Task<bool?> Verify(HttpRequest request, ProtocolMessage data) {
			if(data.ClientId == null || !clientKeys.TryGetValue(data.ClientId, out PublicKey publicKey))
				return null;
			try {
				string signatureHeader = request.Headers["X-Signature"];
				byte[] body = await CopyBody(request);
				return algorithm.Verify(publicKey, body, Convert.FromBase64String(signatureHeader));
			} catch(Exception e) {
				logger.LogError(e, $"Client {data.ClientId} verification failed");
				return null;
			}
		}

		public async Task<string> Authenticate(HttpRequest request, ProtocolCredential credential) {
			try {
				string signatureHeader = request.Headers["X-Signature"];
				(byte[] publicKeyBytes, PublicKey publicKey) = credential.Decode(algorithm);
				byte[] body = await CopyBody(request);
				if(algorithm.Verify(publicKey, body, Convert.FromBase64String(signatureHeader))) {
					string versionMessage = CheckVersion(credential.Version);
					if(versionMessage != null)
						return versionMessage;
					string hash = Convert.ToBase64String(HashAlgorithm.Sha256.Hash(publicKeyBytes));
					clientKeys.TryAdd(hash, publicKey);
					logger.LogInformation($"Authentication successful for client {hash} version {credential.Version}");
					return null;
				}
			} catch(Exception e) {
				logger.LogError(e, $"Authentication failed for {credential.EncodedPublicKey}");
			}
			return "Invalid credentials";
		}

		private async Task<byte[]> CopyBody(HttpRequest request) {
			request.Body.Position = 0;
			byte[] body = new byte[request.Headers.ContentLength ?? 0];
			using MemoryStream stream = new MemoryStream(body);
			await request.Body.CopyToAsync(stream);
			request.Body.Position = 0;
			return body;
		}

		private string CheckVersion(string versionString) {
			if(!Version.TryParse(versionString, out Version version))
				return "Invalid version";
			if(version < minVersion)
				return $"Does not meet minimum required version {minVersion}";
			if(version >= maxVersion)
				return $"Does not meet maximum required version {maxVersion}";
			return null;
		}
	}
}
