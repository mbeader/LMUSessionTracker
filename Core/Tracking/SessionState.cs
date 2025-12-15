using LMUSessionTracker.Core.LMU;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace LMUSessionTracker.Core.Tracking {
	public class SessionState {
		private static readonly Regex filenamePattern = new Regex(@"^\d{17}-[0-9a-f]{8}(?:-[0-9a-f]{4}){3}-[0-9a-f]{12}\.json$", RegexOptions.Compiled);
		private static readonly string datadir = "data";
		public required string SessionId { get; set; }
		public SessionInfo Info { get; set; }
		public List<CarHistory> History { get; set; }

		public static WriteResult Write(string sessionId, SessionInfo info, List<CarHistory> history) {
			try {
				if(!Directory.Exists(datadir))
					Directory.CreateDirectory(datadir);
				string json = JsonConvert.SerializeObject(new SessionState() { SessionId = sessionId, Info = info, History = history });
				File.WriteAllText(Path.Join(datadir, $"{sessionId}.json"), json);
				return new WriteResult();
			} catch(Exception e) {
				return new WriteResult() { Exception = e };
			}
		}

		public static ReadResult<SessionState> Read(string sessionId = null) {
			try {
				if(!Directory.Exists(datadir))
					return new ReadResult<SessionState>();
				string lastFile = null;
				foreach(string file in Directory.EnumerateFiles(datadir)) {
					string name = Path.GetFileName(file);
					if(sessionId != null && name == $"{sessionId}.json") {
						lastFile = file;
						break;
					} else if(filenamePattern.IsMatch(name) && lastFile == null || StringComparer.OrdinalIgnoreCase.Compare(lastFile, file) < 0)
						lastFile = file;
				}
				if(lastFile == null)
					return new ReadResult<SessionState>();
				string json = File.ReadAllText(lastFile);
				return new ReadResult<SessionState>() { Value = JsonConvert.DeserializeObject<SessionState>(json) };
			} catch(Exception e) {
				return new ReadResult<SessionState>() { Exception = e };
			}
		}
	}

	public class WriteResult {
		public bool Success => Exception == null;
		public Exception Exception { get; init; }
	}

	public class ReadResult<T> {
		public bool Success => Exception == null;
		public Exception Exception { get; init; }
		public T Value { get; init; }
	}
}
