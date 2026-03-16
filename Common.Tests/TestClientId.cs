using LMUSessionTracker.Common.Client;
using System;

namespace LMUSessionTracker.Common.Tests {
	public static class TestClientId {
		private static readonly string publicKey = "LS0tLS1CRUdJTiBQVUJMSUMgS0VZLS0tLS0NCk1Db3dCUVlESzJWd0F5RUFTZkR4YUxxV1IxUmMzaVY4ZGxUQVRONW80UmhISTN5YUxhT2RBOGk3OUpjPQ0KLS0tLS1FTkQgUFVCTElDIEtFWS0tLS0tDQo=";
		private static readonly string privateKey = "LS0tLS1CRUdJTiBQUklWQVRFIEtFWS0tLS0tDQpNQzRDQVFBd0JRWURLMlZ3QkNJRUlMNTZOVk5Sa2dFdGxUbS9sY0lmK1FsaWM3YlAySEc2dVVSQUNsekZPdnQ4DQotLS0tLUVORCBQUklWQVRFIEtFWS0tLS0tDQo=";
		private static readonly ClientId clientId;

		public static ClientId ClientId => clientId;

		static TestClientId() {
			clientId = ClientId.Import(Convert.FromBase64String(privateKey));
		}
	}
}
