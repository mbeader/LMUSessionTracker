using System.Collections.Generic;

namespace LMUSessionTracker.Core.Tracking {
	public class SessionDiff {
		private static readonly string defaultMessageFormat = "Expected: {0}. Actual {1}.";

		public string SessionId { get; set; }
		public bool IsSame => Differences.Count == 0 || Differences[0] == SessionDifference.None;
		public List<SessionDifference> Differences { get; set; } = new List<SessionDifference>();
		private List<SessionDifferenceDescriptor> Messages { get; set; } = new List<SessionDifferenceDescriptor>();

		public string GetMessage() {
			if(Messages != null) {
				List<string> messages = new List<string>();
				foreach(SessionDifferenceDescriptor descriptor in Messages)
					messages.Add($"{descriptor.Difference}: [{descriptor.Message}]");
				return string.Join(", ", messages);
			}
			return null;
		}

		public void Set(SessionDifference difference, object expected, object actual) {
			object[] messageParams = new object[2];
			messageParams[0] = expected;
			messageParams[1] = actual;
			Set(difference, defaultMessageFormat, messageParams);
		}

		public void Set(SessionDifference difference, string messageFormat, object[] messageParams = null) {
			Differences.Add(difference);
			Messages.Add(new SessionDifferenceDescriptor(difference, messageFormat, messageParams));
		}

		private readonly struct SessionDifferenceDescriptor {
			public SessionDifference Difference { get; init; }
			public string MessageFormat { get; init; }
			public object[] MessageParams { get; init; }
			public string Message => MessageFormat != null ? MessageParams != null ? string.Format(MessageFormat, MessageParams) : MessageFormat : null;

			public SessionDifferenceDescriptor(SessionDifference difference, string messageFormat, object[] messageParams) {
				Difference = difference;
				MessageFormat = messageFormat;
				MessageParams = messageParams;
			}
		}
	}
}
