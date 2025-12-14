using System;

namespace LMUSessionTracker.Core {
	public static class Format {
		public static string Diff(int laps, double time) {
			return laps > 0 ? $"{laps} L" : time.ToString("0.000");
		}

		public static string Percent(double? value) {
			return !value.HasValue ? "" : value.Value.ToString("P2");
		}

		public static string Time(double? time) {
			return !time.HasValue ? "" : new TimeSpan((long)Math.Round(time.Value * 1000) * TimeSpan.TicksPerMillisecond).ToString("hh':'mm':'ss'.'fff");
		}

		public static string LapTime(double laptime) {
			if(laptime <= 0.0)
				return "-";
			TimeSpan ts = new TimeSpan((long)Math.Round(laptime * 1000) * TimeSpan.TicksPerMillisecond);
			string format = "ss'.'fff";
			if(ts.TotalHours > 0)
				format = "hh':'mm':'ss'.'fff";
			if(ts.TotalMinutes > 0)
				format = "mm':'ss'.'fff";
			return ts.ToString(format);
		}

		public static string Temp(double? c) {
			if(!c.HasValue)
				return "";
			double f = c.Value * 9.0 / 5.0 + 32;
			return $"{c:N1} °C ({f:N1} °F)";
		}
	}
}
