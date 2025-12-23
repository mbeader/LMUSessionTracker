using NSec.Cryptography;
using System;
using System.IO;

namespace LMUSessionTracker.Core.Client {
	public class ClientId {
		private static readonly string datadir = "data";
		private static readonly string privateKeyFile = "client.key";
		private static readonly string publicKeyFile = "client.pem";
		private static readonly SignatureAlgorithm algorithm = SignatureAlgorithm.Ed25519;

		public PublicKey PublicKey { get; private set; }
		public Key PrivateKey { get; private set; }
		public string Hash { get; private set; }

		public ClientId(Key privateKey) {
			PrivateKey = privateKey;
			PublicKey = privateKey.PublicKey;
			Hash = Convert.ToBase64String(HashAlgorithm.Sha256.Hash(PublicKey.Export(KeyBlobFormat.PkixPublicKeyText)));
		}

		public static ClientId LoadOrCreate(string privateKey) {
			if(string.IsNullOrEmpty(privateKey)) {
				string path = Path.Join(datadir, privateKeyFile);
				if(File.Exists(path))
					return Load(path);
				else
					return Create(true);
			} else
				return Load(privateKey);
		}

		public static ClientId Create(bool persist = false) {
			Key key = Key.Create(algorithm, new KeyCreationParameters() { ExportPolicy = KeyExportPolicies.AllowPlaintextExport });
			if(persist) {
				Directory.CreateDirectory(datadir);
				File.WriteAllBytes(Path.Join(datadir, privateKeyFile), key.Export(KeyBlobFormat.PkixPrivateKeyText));
				File.WriteAllBytes(Path.Join(datadir, publicKeyFile), key.Export(KeyBlobFormat.PkixPublicKeyText));
			}
			return new ClientId(key);
		}

		private static ClientId Load(string privateKeyFile) {
			byte[] privateBytes = File.ReadAllBytes(privateKeyFile);
			Key privateKey = Key.Import(algorithm, privateBytes, KeyBlobFormat.PkixPrivateKeyText);
			return new ClientId(privateKey);
		}

		public static ClientId Import(byte[] privateKeyBytes) {
			Key privateKey = Key.Import(algorithm, privateKeyBytes, KeyBlobFormat.PkixPrivateKeyText);
			return new ClientId(privateKey);
		}
	}
}
