namespace LMUSessionTracker.Core.Protocol {
	public enum ProtocolResult {
		/// <summary>
		/// Data consumed, no session change
		/// </summary>
		Accepted,
		/// <summary>
		/// Client out of sync with server
		/// </summary>
		Rejected,
		/// <summary>
		/// Client role changed from secondary to primary
		/// </summary>
		Promotion,
		/// <summary>
		/// Client role changed from primary to secondary
		/// </summary>
		Demotion,
		/// <summary>
		/// Session changed, roles unchanged
		/// </summary>
		Changed
	}
}
