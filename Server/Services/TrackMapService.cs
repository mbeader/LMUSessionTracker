using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Server.Tracking;
using LMUSessionTracker.Server.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;

namespace LMUSessionTracker.Server.Services {
	public class TrackMapService {
		private static readonly string baseresname = "LMUSessionTracker.Server.Tracks.";
		private static readonly ConcurrentDictionary<string, TrackMap> tracks = new ConcurrentDictionary<string, TrackMap>();
		private static readonly Dictionary<string, TrackMapMetadata> trackMetadata = new Dictionary<string, TrackMapMetadata>() {
			{ "Algarve International Circuit", new(64, 260, 583) },
			{ "Autodromo Enzo e Dino Ferrari", new(970, 309, 634) },
			{ "Autodromo Nazionale Monza", new(296, 695, 1055) },
			{ "Autódromo José Carlos Pace", new(1, 261, 637) },
			{ "Bahrain Endurance Circuit", new(36, 478, 691) },
			{ "Bahrain International Circuit", new(1075, 340, 792) },
			{ "Bahrain Outer Circuit", new(17, 357, 492) },
			{ "Bahrain Paddock Circuit", new(9, 256, 490) },
			{ "COTA National Circuit", new(16, 277, 428) },
			{ "Circuit de Spa-Francorchamps Endurance", new(170, 603, 1156) },
			{ "Circuit de Spa-Francorchamps", new(170, 603, 1157) },
			{ "Circuit de la Sarthe Mulsanne", new(207, 586, 1751) },
			{ "Circuit de la Sarthe", new(206, 586, 1751) },
			{ "Circuit of the Americas", new(5, 337, 774) },
			{ "Fuji Speedway Classic", new(122, 495, 712) },
			{ "Fuji Speedway", new(123, 496, 721) },
			{ "Lusail International Circuit", new(1029, 252, 659) },
			{ "Lusail Short Circuit", new(1, 274, 551) },
			{ "Monza Curva Grande Circuit", new(296, 695, 1056) },
			{ "Paul Ricard - ELMS", new(1127, 285, 666) },
			{ "Sebring International Raceway", new(2, 431, 728) },
			{ "Sebring School Circuit", new(269, 476, 584) },
			{ "Silverstone Grand Prix Circuit - ELMS", new(1149, 290, 632) },
		};
		private readonly ILogger<TrackMapService> logger;

		public TrackMapService(ILogger<TrackMapService> logger) {
			this.logger = logger;
		}

		public virtual TrackMap GetTrack(string name) {
			if(tracks.TryGetValue(name, out TrackMap track))
				return track;
			List<TrackMapPoint> points;
			try {
				points = LoadTrack(name);
			} catch(Exception e) {
				logger.LogWarning(e, $"Failed to read track map: {name}");
				return null;
			}
			if(points == null || points.Count == 0)
				return null;
			track = ProcessPoints(points);
			if(trackMetadata.TryGetValue(name, out TrackMapMetadata metadata))
				SetMetadata(track, metadata.S1Index, metadata.S2Index, metadata.S3Index);
			return tracks.GetOrAdd(name, track);
		}

		private static List<TrackMapPoint> LoadTrack(string name) {
			var assembly = Assembly.GetExecutingAssembly();
			using Stream stream = assembly.GetManifestResourceStream($"{baseresname}{name}.json");
			return JsonSerializer.Deserialize<List<TrackMapPoint>>(stream, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
		}

		private static TrackMap ProcessPoints(List<TrackMapPoint> points) {
			TrackMap track = new TrackMap() { Points = new List<Point2D>(), Pits = new List<Point2D>() };
			if(points.Count > 0) {
				track.MaxX = points[0].X;
				track.MaxY = points[0].Y;
				track.MinX = points[0].X;
				track.MinY = points[0].Y;
			}
			foreach(TrackMapPoint tmPoint in points) {
				Point2D point = new Point2D(tmPoint);
				switch(tmPoint.Type) {
					case 0:
						track.Points.Add(point);
						break;
					case 1:
						track.Pits.Add(point);
						break;
					default:
						continue;
				}
				if(point.X > track.MaxX)
					track.MaxX = point.X;
				else if(point.X < track.MinX)
					track.MinX = point.X;
				if(point.Y > track.MaxY)
					track.MaxY = point.Y;
				else if(point.Y < track.MinY)
					track.MinY = point.Y;
			}
			return track;
		}

		public virtual bool NeedsMetadata(string name) {
			return tracks.TryGetValue(name, out TrackMap track) && track.Points != null;
		}

		public virtual void SetMetadata(string name, int s1Index, int s2Index, int s3Index) {
			if(tracks.TryGetValue(name, out TrackMap track) && track.Points != null)
				SetMetadata(track, s1Index, s2Index, s3Index);
		}

		private void SetMetadata(TrackMap track, int s1Index, int s2Index, int s3Index) {
			track.S1 = new List<Point2D>();
			track.S2 = new List<Point2D>();
			track.S3 = new List<Point2D>();
			int min = Math.Min(s1Index, Math.Min(s2Index, s3Index));
			int sector = s1Index == min ? 1 : s2Index == min ? 2 : 3;
			if(min != 0)
				sector--;
			if(sector == 0)
				sector = 3;
			int nextSector = sector == 3 ? s1Index : sector == 1 ? s2Index : s3Index;
			for(int i = 0; i < track.Points.Count; i++) {
				int curr = (s1Index + i) % track.Points.Count;
				if(curr == nextSector) {
					sector = (sector + 1) % 3;
					nextSector = sector == 3 ? s1Index : sector == 1 ? s2Index : s3Index;
				}
				if(sector == 1)
					track.S1.Add(track.Points[curr]);
				else if(sector == 2)
					track.S2.Add(track.Points[curr]);
				else
					track.S3.Add(track.Points[curr]);
			}
			track.Points = null;
		}
	}
}
