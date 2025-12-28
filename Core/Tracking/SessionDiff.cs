namespace LMUSessionTracker.Core.Tracking {
	public class SessionDiff {
		public string SessionId { get; set; }
		public bool IsSame => Difference == SessionDifference.None;
		public SessionDifference Difference { get; set; }
		public string MessageFormat { get; set; }
		public object[] MessageParams { get; set; }

		public string GetMessage() => MessageFormat != null ? MessageParams != null ? string.Format(MessageFormat, MessageParams) : MessageFormat : null;

		public void Set(SessionDifference difference, object expected, object actual) {
			Difference = difference;
			MessageParams ??= new object[2];
			MessageParams[0] = expected;
			MessageParams[1] = actual;
		}
	}
}
