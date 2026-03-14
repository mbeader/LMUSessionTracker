using LMUSessionTracker.Common.LMU;
using System.Collections.Generic;

namespace LMUSessionTracker.Common.Protocol {
	public class ProtocolState {
		public Chat Chat { get; set; }
		public List<ProtocolCarState> Cars { get; set; }
	}

	public class ProtocolCarState {
		public int SlotId { get; set; }
		public string Veh { get; set; }
		public string Team { get; set; }
		public string Driver { get; set; }
		public int LastResolvedPitLap { get; set; }
	}
}
