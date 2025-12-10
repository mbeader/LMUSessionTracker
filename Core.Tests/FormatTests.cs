namespace LMUSessionTracker.Core.Tests {
	public class FormatTests {
		[Fact]
		public void Diff_LeadLap_ReturnsTime() {
			Assert.Equal("1.235", Format.Diff(0, 1.2345));
		}

		[Fact]
		public void Diff_LeadLap_ReturnsLaps() {
			Assert.Equal("1 L", Format.Diff(1, 1.2345));
		}
	}
}
