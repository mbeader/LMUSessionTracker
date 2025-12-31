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
		public Task<bool> Authenticate(HttpRequest request, ProtocolCredential credential);
	}

	public class DefaultProtocolAuthenticator : ProtocolAuthenticator {
		private static readonly SignatureAlgorithm algorithm = SignatureAlgorithm.Ed25519;
		private readonly ConcurrentDictionary<string, PublicKey> clientKeys = new ConcurrentDictionary<string, PublicKey>();
		private readonly ILogger<DefaultProtocolAuthenticator> logger;

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

		public async Task<bool> Authenticate(HttpRequest request, ProtocolCredential credential) {
			try {
				string signatureHeader = request.Headers["X-Signature"];
				(byte[] publicKeyBytes, PublicKey publicKey) = credential.Decode(algorithm);
				byte[] body = await CopyBody(request);
				if(algorithm.Verify(publicKey, body, Convert.FromBase64String(signatureHeader))) {
					string hash = Convert.ToBase64String(HashAlgorithm.Sha256.Hash(publicKeyBytes));
					clientKeys.TryAdd(hash, publicKey);
					logger.LogInformation($"Authentication successful for client {hash} version {credential.Version}");
					return true;
				} else
					return false;
			} catch(Exception e) {
				logger.LogError(e, $"Authentication failed for {credential.EncodedPublicKey}");
				return false;
			}
		}

		private async Task<byte[]> CopyBody(HttpRequest request) {
			request.Body.Position = 0;
			byte[] body = new byte[request.Headers.ContentLength ?? 0];
			using MemoryStream stream = new MemoryStream(body);
			await request.Body.CopyToAsync(stream);
			request.Body.Position = 0;
			return body;
		}
	}
}
