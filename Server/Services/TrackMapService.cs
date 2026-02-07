using LMUSessionTracker.Core.LMU;
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
	}
}
