namespace LMUSessionTracker.Core.Protocol {
	public enum ProtocolRole {
		/// <summary>
		/// Cannot send data
		/// </summary>
		None,
		/// <summary>
		/// Sends main data
		/// </summary>
		Primary,
		/// <summary>
		/// Follows session status
		/// </summary>
		Secondary
	}
}
