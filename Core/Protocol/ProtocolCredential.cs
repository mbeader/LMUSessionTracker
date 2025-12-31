using NSec.Cryptography;
using System;

namespace LMUSessionTracker.Core.Protocol {
	public class ProtocolCredential {
		public string EncodedPublicKey { get; set; }
		public string Version { get; set; }

		public ProtocolCredential() { }

		public ProtocolCredential(PublicKey publicKey, Version version) {
			Encode(publicKey);
			Version = version.ToString();
		}

		public void Encode(PublicKey publicKey) {
			EncodedPublicKey = Convert.ToBase64String(publicKey.Export(KeyBlobFormat.PkixPublicKeyText));
		}

		public (byte[] publicKeyBytes, PublicKey publicKey) Decode(SignatureAlgorithm algorithm) {
			byte[] publicKeyBytes = Convert.FromBase64String(EncodedPublicKey);
			PublicKey publicKey = PublicKey.Import(algorithm, publicKeyBytes, KeyBlobFormat.PkixPublicKeyText);
			return (publicKeyBytes, publicKey);
		}
	}
}
