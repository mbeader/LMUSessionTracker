using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Core.Tracking;
using LMUSessionTracker.Server.Services;
using LMUSessionTracker.Server.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace LMUSessionTracker.Server.Tracking {
	public class TrackMapBuilder : Core.Tracking.TrackMapBuilder {
		private static readonly int minSampleSize = 10;
		private static readonly int minVotes = 5;
		private readonly Dictionary<string, Dictionary<CarKey, BuilderState>> state = new Dictionary<string, Dictionary<CarKey, BuilderState>>();
		private readonly Dictionary<string, List<SectorMarkers>> trackSectors = new Dictionary<string, List<SectorMarkers>>();
		private readonly ILogger<TrackMapBuilder> logger;
		private readonly TrackMapService trackMapService;

		public TrackMapBuilder(ILogger<TrackMapBuilder> logger, TrackMapService trackMapService) {
			this.logger = logger;
			this.trackMapService = trackMapService;
		}

		public void Update(string sessionId, string track, List<Standing> standings) {
			if(!trackMapService.NeedsMetadata(track))
				return;
			if(!state.TryGetValue(sessionId, out Dictionary<CarKey, BuilderState> session)) {
				session = new Dictionary<CarKey, BuilderState>();
				state.Add(sessionId, session);
			}
			if(!trackSectors.TryGetValue(track, out List<SectorMarkers> sectors)) {
				sectors = new List<SectorMarkers>();
				trackSectors.Add(track, sectors);
			}
			int prevCount = sectors.Count;
			foreach(Standing standing in standings) {
				CarKey key = new CarKey(standing.slotID, standing.vehicleFilename);
				if(!session.TryGetValue(key, out BuilderState car)) {
					car = new BuilderState() { Lap = standing.lapsCompleted, Valid = false };
					session.Add(key, car);
				} else {
					SectorMarkers newSectors = Update(car, standing);
					if(newSectors != null)
						sectors.Add(newSectors);
				}
			}
			if(sectors.Count != prevCount && sectors.Count >= minSampleSize) {
				TrackMap trackMap = trackMapService.GetTrack(track);
				if(trackMap != null) {
					TrackMapMetadata metadata = Vote(trackMap, sectors);
					if(metadata != null) {
						trackMapService.SetMetadata(track, metadata.S1Index, metadata.S2Index, metadata.S3Index);
						logger.LogDebug($"Sector markers for {track}: {metadata.S1Index} {metadata.S2Index} {metadata.S3Index}");
					}
				}
			}
		}

		private SectorMarkers Update(BuilderState state, Standing standing) {
			SectorMarkers res = null;
			if(standing.lapsCompleted == state.Lap) {
				AddPoint(state, standing);
			} else {
				if(standing.lapsCompleted > 1 && standing.lapsCompleted == state.Lap + 1 && standing.lastLapTime > 0 && state.Valid) {
					res = new SectorMarkers() { S1 = state.S1[0], S2 = state.S2[0], S3 = state.S3[0] };
				}
				state.Reset(standing.lapsCompleted);
				AddPoint(state, standing);
			}
			return res;
		}

		private void AddPoint(BuilderState state, Standing standing) {
			Point2D point = new Point2D() { X = standing.carPosition.x, Y = standing.carPosition.z };
			switch(standing.sector) {
				case "SECTOR1":
					state.S1.Add(point);
					break;
				case "SECTOR2":
					state.S2.Add(point);
					break;
				case "SECTOR3":
					state.S3.Add(point);
					break;
			}
		}

		private TrackMapMetadata Vote(TrackMap trackMap, List<SectorMarkers> sectors) {
			TrackMapMetadata res = null;
			List<int> s1Votes = new List<int>();
			List<int> s2Votes = new List<int>();
			List<int> s3Votes = new List<int>();
			foreach(SectorMarkers markers in sectors) {
				s1Votes.Add(CalcMinDistance(markers.S1, trackMap.Points));
				s2Votes.Add(CalcMinDistance(markers.S2, trackMap.Points));
				s3Votes.Add(CalcMinDistance(markers.S3, trackMap.Points));
			}
			int? s1Vote = CountVotes(s1Votes);
			int? s2Vote = CountVotes(s2Votes);
			int? s3Vote = CountVotes(s3Votes);
			if(s1Vote.HasValue && s2Vote.HasValue && s3Vote.HasValue)
				res = new TrackMapMetadata() { S1Index = s1Vote.Value, S2Index = s2Vote.Value, S3Index = s3Vote.Value };
			return res;
		}

		private int? CountVotes(List<int> votes) {
			votes.Sort();
			int mode = -1;
			int modeFreq = 0;
			int freqCount = 0;
			int currFreq = 0;
			for(int i = 0; i < votes.Count; i++) {
				if(i == 0) {
					mode = votes[i];
					modeFreq = 1;
					freqCount = 1;
					currFreq = 1;
				} else {
					if(votes[i-1] == votes[i]) {
						currFreq++;
						if(currFreq == modeFreq) {
							freqCount++;
						} else if(currFreq > modeFreq) {
							mode = votes[i];
							modeFreq = currFreq;
							freqCount = 1;
						}
					} else {
						currFreq = 1;
						if(currFreq == modeFreq)
							freqCount++;
					}
				}
			}
			return mode >= 0 && modeFreq >= minVotes && freqCount == 1 ? mode : null;
		}

		private int CalcMinDistance(Point2D marker, List<Point2D> points) {
			double min = double.MaxValue;
			int minIndex = -1;
			for(int i = 0; i < points.Count; i++) {
				Point2D point = points[i];
				double dist = Math.Sqrt(Math.Pow(marker.X - point.X, 2) + Math.Pow(marker.Y - point.Y, 2));
				if(dist < min) {
					min = dist;
					minIndex = i;
				}
			}
			return minIndex;
		}

		private class SectorMarkers {
			public Point2D S1 { get; set; }
			public Point2D S2 { get; set; }
			public Point2D S3 { get; set; }
		}

		private class BuilderState {
			public int Lap { get; set; }
			public List<Point2D> S1 { get; set; } = new List<Point2D>();
			public List<Point2D> S2 { get; set; } = new List<Point2D>();
			public List<Point2D> S3 { get; set; } = new List<Point2D>();
			public bool Valid { get; set; }

			public void Reset(int lap) {
				Lap = lap;
				S1.Clear();
				S2.Clear();
				S3.Clear();
				Valid = true;
			}
		}
	}
}
