using LMUSessionTracker.Core.Protocol;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Moq;
using Newtonsoft.Json;
using NSec.Cryptography;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace LMUSessionTracker.Core.Tests.Protocol {
	public class ProtocolAuthenticatorTests {
		private static readonly SignatureAlgorithm algorithm = SignatureAlgorithm.Ed25519;
		private readonly DefaultProtocolAuthenticator protocolAuthenticator;
		private readonly Key privateKey;

		public ProtocolAuthenticatorTests(LoggingFixture loggingFixture) {
			protocolAuthenticator = new DefaultProtocolAuthenticator(loggingFixture.LoggerFactory.CreateLogger<DefaultProtocolAuthenticator>());
			privateKey = Key.Create(algorithm, new KeyCreationParameters() { ExportPolicy = KeyExportPolicies.AllowPlaintextExport });
		}

		private HttpRequest Request(object content, string signature) {
			Mock<HttpRequest> request = new Mock<HttpRequest>();
			Dictionary<string, StringValues> headers = new Dictionary<string, StringValues>();
			if(signature != null)
				headers.Add("X-Signature", signature);
			MemoryStream stream = new MemoryStream(Body(content));
			headers.Add("Content-Length", stream.Length.ToString());
			request.Setup(x => x.Headers).Returns(new HeaderDictionary(headers));
			request.Setup(x => x.Body).Returns(stream);
			return request.Object;
		}

		private byte[] Body(object content) {
			return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(content));
		}

		private string Base64(string content) {
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(content));
		}

		private async Task<bool> Authenticate(string publicKey, string signature) {
			return await protocolAuthenticator.Authenticate(Request(publicKey, signature), publicKey);
		}

		[Fact]
		public async Task Authenticate_NullClientId_ReturnsFalse() {
			Assert.False(await Authenticate(null, null));
		}

		[Fact]
		public async Task Authenticate_EmptyClientId_ReturnsFalse() {
			Assert.False(await Authenticate("", null));
		}

		[Fact]
		public async Task Authenticate_InvalidClientIdEncoding_ReturnsFalse() {
			Assert.False(await Authenticate("notakey", null));
		}

		[Fact]
		public async Task Authenticate_InvalidClientId_ReturnsFalse() {
			Assert.False(await Authenticate(Base64("notakey"), null));
		}

		[Fact]
		public async Task Authenticate_NullSignature_ReturnsFalse() {
			Assert.False(await Authenticate(Convert.ToBase64String(privateKey.PublicKey.Export(KeyBlobFormat.PkixPublicKeyText)), null));
		}

		[Fact]
		public async Task Authenticate_EmptySignature_ReturnsFalse() {
			Assert.False(await Authenticate(Convert.ToBase64String(privateKey.PublicKey.Export(KeyBlobFormat.PkixPublicKeyText)), ""));
		}

		[Fact]
		public async Task Authenticate_InvalidSignatureEncoding_ReturnsFalse() {
			Assert.False(await Authenticate(Convert.ToBase64String(privateKey.PublicKey.Export(KeyBlobFormat.PkixPublicKeyText)), "notasig"));
		}

		[Fact]
		public async Task Authenticate_InvalidSignature_ReturnsFalse() {
			Assert.False(await Authenticate(Convert.ToBase64String(privateKey.PublicKey.Export(KeyBlobFormat.PkixPublicKeyText)), Base64("notasig")));
		}

		[Fact]
		public async Task Authenticate_MismatchSignature_ReturnsFalse() {
			Key otherKey = Key.Create(algorithm, new KeyCreationParameters() { ExportPolicy = KeyExportPolicies.AllowPlaintextExport });
			byte[] sig = algorithm.Sign(otherKey, privateKey.PublicKey.Export(KeyBlobFormat.PkixPublicKeyText));
			Assert.False(await Authenticate(Convert.ToBase64String(privateKey.PublicKey.Export(KeyBlobFormat.PkixPublicKeyText)), Convert.ToBase64String(sig)));
		}

		[Fact]
		public async Task Authenticate_ValidSignature_ReturnsTrue() {
			byte[] sig = algorithm.Sign(privateKey, Body(Convert.ToBase64String(privateKey.PublicKey.Export(KeyBlobFormat.PkixPublicKeyText))));
			Assert.True(await Authenticate(Convert.ToBase64String(privateKey.PublicKey.Export(KeyBlobFormat.PkixPublicKeyText)), Convert.ToBase64String(sig)));
		}

		private async Task<bool?> Verify(ProtocolMessage data, string signature, bool preAuthenticate = true) {
			data ??= new() { ClientId = Convert.ToBase64String(HashAlgorithm.Sha256.Hash(privateKey.PublicKey.Export(KeyBlobFormat.PkixPublicKeyText))) };
			string publicKey = Convert.ToBase64String(privateKey.PublicKey.Export(KeyBlobFormat.PkixPublicKeyText));
			await Authenticate(publicKey, Convert.ToBase64String(algorithm.Sign(privateKey, Body(publicKey))));
			return await protocolAuthenticator.Verify(Request(data, signature), data);
		}

		[Fact]
		public async Task Verify_NullClientId_ReturnsNull() {
			Assert.Null(await Verify(new() { ClientId = null }, null));
		}

		[Fact]
		public async Task Verify_NullSignature_ReturnsNull() {
			Assert.Null(await Verify(null, null));
		}

		[Fact]
		public async Task Verify_EmptySignature_ReturnsFalse() {
			Assert.False(await Verify(null, ""));
		}

		[Fact]
		public async Task Verify_InvalidSignatureEncoding_ReturnsNull() {
			Assert.Null(await Verify(null, "notasig"));
		}

		[Fact]
		public async Task Verify_InvalidSignature_ReturnsFalse() {
			Assert.False(await Verify(null, Base64("notasig")));
		}

		[Fact]
		public async Task Verify_InvalidClientId_ReturnsNull() {
			byte[] sig = algorithm.Sign(privateKey, Body("someothercontent"));
			Assert.Null(await Verify(new() { ClientId = "notahash" }, Convert.ToBase64String(sig)));
		}

		[Fact]
		public async Task Verify_ValidSignature_ReturnsTrue() {
			ProtocolMessage data = new() { ClientId = Convert.ToBase64String(HashAlgorithm.Sha256.Hash(privateKey.PublicKey.Export(KeyBlobFormat.PkixPublicKeyText))) };
			byte[] sig = algorithm.Sign(privateKey, Body(data));
			Assert.True(await Verify(data, Convert.ToBase64String(sig)));
		}
	}
}
