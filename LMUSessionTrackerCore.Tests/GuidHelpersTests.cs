using System;

namespace LMUSessionTracker.Core.Tests {
	public class GuidHelpersTests {
		[Fact]
		public void CreateVersion7_FromTimestamp_MatchesFormat() {
			DateTimeOffset dt = new DateTimeOffset(2025, 12, 9, 17, 12, 34, 567, TimeSpan.Zero);
			ulong unixts = 1765300354567L;
			byte[] ex = BitConverter.GetBytes(unixts);
			Guid guid = GuidHelpers.CreateVersion7(dt);
			byte[] ac = guid.ToByteArray();
			Assert.Equal(ex[2], ac[0]);
			Assert.Equal(ex[3], ac[1]);
			Assert.Equal(ex[4], ac[2]);
			Assert.Equal(ex[5], ac[3]);
			Assert.Equal(ex[0], ac[4]);
			Assert.Equal(ex[1], ac[5]);
			Assert.Equal(0x70, ac[7] & 0x70);
			Assert.Equal(0x80, ac[8] & 0x80);
		}
	}
}
