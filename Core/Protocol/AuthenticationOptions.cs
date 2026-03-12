using System.Collections.Generic;

namespace LMUSessionTracker.Core.Protocol {
	public class AuthenticationOptions {
		public bool UseWhitelist { get; set; } = false;
		public List<string> ClientWhitelist { get; set; } = null;
	}
}
