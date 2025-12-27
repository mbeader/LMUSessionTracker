using LMUSessionTracker.Core.LMU;
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
			return !time.HasValue ? "" : new TimeSpan((long)Math.Round(time.Value * 1000) * TimeSpan.TicksPerMillisecond).ToString("hh':'mm':'ss");
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

		public static string SectorTime(double start, double end) {
			if(start < 0.0 || end <= 0.0)
				return "-";
			return LapTime(end - start);
		}

		public static string Temp(double? c) {
			if(!c.HasValue)
				return "";
			double f = c.Value * 9.0 / 5.0 + 32;
			return $"{c:N1} °C ({f:N1} °F)";
		}

		public static string Phase(int? gamePhase) {
			switch(gamePhase) {
				case 0: return "Starting";
				case 1: return "Reconnaissance laps";
				case 2: return "Grid";
				case 3: return "Formation lap";
				case 4: return "Countdown";
				case 5: return "Green";
				case 6: return "FCY";
				case 7: return "Session stopped";
				case 8: return "Checkered";
				case 9: return "Paused";
				default: return $"Unknown ({gamePhase})";
			}
		}

		// indicating pit should be done on lap with entering status
		public static string Status(Standing standing) {
			if(standing.inGarageStall)
				return "Gar";
			else if(standing.pitState == "NONE")
				return "Run";
			else if(standing.pitState == "ENTERING")
				return "In";
			else if(standing.pitState == "EXITING")
				return "Out";
			else if(standing.pitState == "STOPPED")
				return "Pit";
			else if(standing.pitState == "REQUEST")
				return "Req";
			else
				return "???";
		}

		public static string RelativeTimestamp(DateTime now, DateTime timestamp) {
			TimeSpan span = now - timestamp;
			if(span.TotalSeconds < 10)
				return "now";
			else if(span.TotalMinutes < 1)
				return $"{span.TotalSeconds:N0} seconds ago";
			else if(span.TotalHours < 1)
				return $"{span.TotalMinutes:N0} minutes ago";
			else if(span.TotalDays < 1)
				return $"{span.TotalHours:N0} hours ago";
			else if(span.TotalDays < 31)
				return $"{span.TotalDays:N0} days ago";
			else
				return "long ago";
		}
	}
}
