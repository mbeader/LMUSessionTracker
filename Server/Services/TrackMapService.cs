using System.Collections.Concurrent;
using System.IO;
using System.Reflection;

namespace LMUSessionTracker.Server.Services {
	public class TrackMapService {
		private static readonly string baseresname = "LMUSessionTracker.Server.Tracks.";
		private static readonly ConcurrentDictionary<string, string> tracks = new ConcurrentDictionary<string, string>();

		public virtual string GetTrack(string name) {
			if(tracks.TryGetValue(name, out string track))
				return track;
			try {
				track = LoadTrack(name);
				return tracks.GetOrAdd(name, track);
			} catch {
				return null;
			}
		}

		private static string LoadTrack(string name) {
			var assembly = Assembly.GetExecutingAssembly();
			using Stream stream = assembly.GetManifestResourceStream($"{baseresname}{name}.json");
			using StreamReader reader = new StreamReader(stream);
			return reader.ReadToEnd();
		}
	}
}
