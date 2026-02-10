using LMUSessionTracker.Core.Protocol;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
		private readonly AuthenticationOptions options;

		public ProtocolAuthenticatorTests(LoggingFixture loggingFixture) {
			options = new AuthenticationOptions();
			Mock<IOptions<AuthenticationOptions>> mockOptions = new Mock<IOptions<AuthenticationOptions>>();
			mockOptions.Setup(x => x.Value).Returns(() => options);
			protocolAuthenticator = new DefaultProtocolAuthenticator(loggingFixture.LoggerFactory.CreateLogger<DefaultProtocolAuthenticator>(), mockOptions.Object);
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

		private async Task<string> Authenticate(string publicKey, string signature) {
			ProtocolCredential credential = new ProtocolCredential() { EncodedPublicKey = publicKey, Version = DefaultProtocolAuthenticator.MinVersion.ToString() };
			return await protocolAuthenticator.Authenticate(Request(credential, signature), credential);
		}

		private async Task<string> Authenticate(PublicKey publicKey, string signature) {
			ProtocolCredential credential = new ProtocolCredential(publicKey, DefaultProtocolAuthenticator.MinVersion);
			return await Authenticate(credential, signature);
		}

		private async Task<string> Authenticate(ProtocolCredential credential, string signature) {
			return await protocolAuthenticator.Authenticate(Request(credential, signature), credential);
		}

		[Fact]
		public async Task Authenticate_NullClientId_ReturnsNotNull() {
			Assert.NotNull(await Authenticate((string)null, null));
		}

		[Fact]
		public async Task Authenticate_EmptyClientId_ReturnsNotNull() {
			Assert.NotNull(await Authenticate("", null));
		}

		[Fact]
		public async Task Authenticate_InvalidClientIdEncoding_ReturnsNotNull() {
			Assert.NotNull(await Authenticate("notakey", null));
		}

		[Fact]
		public async Task Authenticate_InvalidClientId_ReturnsNotNull() {
			Assert.NotNull(await Authenticate(Base64("notakey"), null));
		}

		[Fact]
		public async Task Authenticate_NullSignature_ReturnsNotNull() {
			Assert.NotNull(await Authenticate(Convert.ToBase64String(privateKey.PublicKey.Export(KeyBlobFormat.PkixPublicKeyText)), null));
		}

		[Fact]
		public async Task Authenticate_EmptySignature_ReturnsNotNull() {
			Assert.NotNull(await Authenticate(Convert.ToBase64String(privateKey.PublicKey.Export(KeyBlobFormat.PkixPublicKeyText)), ""));
		}

		[Fact]
		public async Task Authenticate_InvalidSignatureEncoding_ReturnsNotNull() {
			Assert.NotNull(await Authenticate(Convert.ToBase64String(privateKey.PublicKey.Export(KeyBlobFormat.PkixPublicKeyText)), "notasig"));
		}

		[Fact]
		public async Task Authenticate_InvalidSignature_ReturnsNotNull() {
			Assert.NotNull(await Authenticate(Convert.ToBase64String(privateKey.PublicKey.Export(KeyBlobFormat.PkixPublicKeyText)), Base64("notasig")));
		}

		[Fact]
		public async Task Authenticate_MismatchSignature_ReturnsNotNull() {
			Key otherKey = Key.Create(algorithm, new KeyCreationParameters() { ExportPolicy = KeyExportPolicies.AllowPlaintextExport });
			ProtocolCredential credential = new ProtocolCredential(privateKey.PublicKey, DefaultProtocolAuthenticator.MinVersion);
			byte[] sig = algorithm.Sign(otherKey, Body(credential));
			Assert.NotNull(await Authenticate(credential, Convert.ToBase64String(sig)));
		}

		[Fact]
		public async Task Authenticate_VersionInvalid_ReturnsNotNull() {
			ProtocolCredential credential = new ProtocolCredential() { Version = "foo" };
			credential.Encode(privateKey.PublicKey);
			byte[] sig = algorithm.Sign(privateKey, Body(credential));
			Assert.NotNull(await Authenticate(credential, Convert.ToBase64String(sig)));
		}

		[Fact]
		public async Task Authenticate_VersionTooLow_ReturnsNotNull() {
			Version min = DefaultProtocolAuthenticator.MinVersion;
			Version ver;
			if(min.Major > 0)
				ver = new Version(min.Major - 1, 0, 0);
			else if(min.Minor > 0)
				ver = new Version(0, min.Minor - 1, 0);
			else
				ver = new Version(0, 0, min.Build - 1);
			ProtocolCredential credential = new ProtocolCredential(privateKey.PublicKey, ver);
			byte[] sig = algorithm.Sign(privateKey, Body(credential));
			Assert.NotNull(await Authenticate(credential, Convert.ToBase64String(sig)));
		}

		[Fact]
		public async Task Authenticate_VersionTooHigh_ReturnsNotNull() {
			ProtocolCredential credential = new ProtocolCredential(privateKey.PublicKey, DefaultProtocolAuthenticator.MaxVersion);
			byte[] sig = algorithm.Sign(privateKey, Body(credential));
			Assert.NotNull(await Authenticate(credential, Convert.ToBase64String(sig)));
		}

		[Fact]
		public async Task Authenticate_NotWhitelisted_ReturnsNotNull() {
			options.UseWhitelist = true;
			options.ClientWhitelist = new List<string>() { "foo" };
			ProtocolCredential credential = new ProtocolCredential(privateKey.PublicKey, DefaultProtocolAuthenticator.MinVersion);
			byte[] sig = algorithm.Sign(privateKey, Body(credential));
			Assert.NotNull(await Authenticate(credential, Convert.ToBase64String(sig)));
		}

		[Fact]
		public async Task Authenticate_ValidSignature_ReturnsNull() {
			ProtocolCredential credential = new ProtocolCredential(privateKey.PublicKey, DefaultProtocolAuthenticator.MinVersion);
			byte[] sig = algorithm.Sign(privateKey, Body(credential));
			Assert.Null(await Authenticate(credential, Convert.ToBase64String(sig)));
		}

		[Fact]
		public async Task Authenticate_ValidSignatureWhitlistedHash_ReturnsNull() {
			options.UseWhitelist = true;
			options.ClientWhitelist = new List<string>() { Convert.ToBase64String(HashAlgorithm.Sha256.Hash(privateKey.PublicKey.Export(KeyBlobFormat.PkixPublicKeyText))) };
			ProtocolCredential credential = new ProtocolCredential(privateKey.PublicKey, DefaultProtocolAuthenticator.MinVersion);
			byte[] sig = algorithm.Sign(privateKey, Body(credential));
			Assert.Null(await Authenticate(credential, Convert.ToBase64String(sig)));
		}

		[Fact]
		public async Task Authenticate_ValidSignatureWhitlistedPublicKey_ReturnsNull() {
			options.UseWhitelist = true;
			options.ClientWhitelist = new List<string>() { "MCowBQYDK2VwAyEA" + Convert.ToBase64String(privateKey.PublicKey.Export(KeyBlobFormat.RawPublicKey)) };
			ProtocolCredential credential = new ProtocolCredential(privateKey.PublicKey, DefaultProtocolAuthenticator.MinVersion);
			byte[] sig = algorithm.Sign(privateKey, Body(credential));
			Assert.Null(await Authenticate(credential, Convert.ToBase64String(sig)));
		}

		private async Task<bool?> Verify(ProtocolMessage data, string signature, bool preAuthenticate = true) {
			data ??= new() { ClientId = Convert.ToBase64String(HashAlgorithm.Sha256.Hash(privateKey.PublicKey.Export(KeyBlobFormat.PkixPublicKeyText))) };
			ProtocolCredential credential = new ProtocolCredential(privateKey.PublicKey, DefaultProtocolAuthenticator.MinVersion);
			await Authenticate(credential, Convert.ToBase64String(algorithm.Sign(privateKey, Body(credential))));
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
