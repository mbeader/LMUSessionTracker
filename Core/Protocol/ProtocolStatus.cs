using System.Text.Json.Serialization;

namespace LMUSessionTracker.Core.Protocol {
	public class ProtocolStatus {
		public string SessionId { get; set; }
		[JsonConverter(typeof(JsonStringEnumConverter<ProtocolRole>))]
		public ProtocolRole Role { get; set; }
		[JsonConverter(typeof(JsonStringEnumConverter<ProtocolResult>))]
		public ProtocolResult Result { get; set; }
	}
}
