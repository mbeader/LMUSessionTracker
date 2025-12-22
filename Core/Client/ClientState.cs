namespace LMUSessionTracker.Core.Client {
	public enum ClientState {
		/// <summary>
		/// LMU is not in an active session, no data to send, not connected to ST server
		/// </summary>
		Idle,
		/// <summary>
		/// LMU is in an active session, connected to ST server but not primary data provider
		/// </summary>
		Connected,
		/// <summary>
		/// LMU is in an active session, not connected to ST server
		/// </summary>
		Disconnected,
		/// <summary>
		/// LMU is in an active session, connected to ST server as primary data provider
		/// </summary>
		Working
	}
}
