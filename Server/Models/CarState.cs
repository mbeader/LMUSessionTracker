using LMUSessionTracker.Core.Tracking;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LMUSessionTracker.Server.Models {
	public class CarState {
		[Key, Required]
		public long CarStateId { get; set; }
		[ForeignKey(nameof(Car)), Required]
		public long CarId { get; set; }
		[ForeignKey(nameof(Session)), Required]
		public string SessionId { get; set; }

		public string CountLapFlag { get; set; }
		public string DriverName { get; set; }
		public string FinishStatus { get; set; }
		public bool InGarageStall { get; set; }
		public double LapStartET { get; set; }
		public int LapsCompleted { get; set; }
		public int Penalties { get; set; }
		public string PitState { get; set; }
		public int Pitstops { get; set; }
		public bool Pitting { get; set; }
		public int Position { get; set; }
		public bool ServerScored { get; set; }

		public int LastPitLap { get; set; }
		public double LastPitTime { get; set; }
		public bool PitThisLap { get; set; }
		public bool GarageThisLap { get; set; }
		public int LastSwapLap { get; set; }
		public double LastSwapTime { get; set; }
		public bool SwapThisLap { get; set; }
		public int SwapLocation { get; set; }
		public int TotalPenalties { get; set; }
		public int TotalPitstops { get; set; }

		[JsonIgnore]
		public Car Car { get; set; }
		[JsonIgnore]
		public Session Session { get; set; }

		public void From(Core.Tracking.CarState state) {
			CountLapFlag = state.CountLapFlag;
			DriverName = state.DriverName;
			FinishStatus = state.FinishStatus;
			InGarageStall = state.InGarageStall;
			LapStartET = state.LapStartET;
			LapsCompleted = state.LapsCompleted;
			Penalties = state.Penalties;
			PitState = state.PitState;
			Pitstops = state.Pitstops;
			Pitting = state.Pitting;
			Position = state.Position;
			ServerScored = state.ServerScored;

			LastPitLap = state.LastPitLap;
			LastPitTime = state.LastPitTime;
			PitThisLap = state.PitThisLap;
			GarageThisLap = state.GarageThisLap;
			LastSwapLap = state.LastSwapLap;
			LastSwapTime = state.LastSwapTime;
			SwapThisLap = state.SwapThisLap;
			SwapLocation = state.SwapLocation;
			TotalPenalties = state.TotalPenalties;
			TotalPitstops = state.TotalPitstops;
		}

		public Core.Tracking.CarState To(CarKey key) {
			return new Core.Tracking.CarState(key) {
				CountLapFlag = CountLapFlag,
				DriverName = DriverName,
				FinishStatus = FinishStatus,
				InGarageStall = InGarageStall,
				LapStartET = LapStartET,
				LapsCompleted = LapsCompleted,
				Penalties = Penalties,
				PitState = PitState,
				Pitstops = Pitstops,
				Pitting = Pitting,
				Position = Position,
				ServerScored = ServerScored,

				LastPitLap = LastPitLap,
				LastPitTime = LastPitTime,
				PitThisLap = PitThisLap,
				GarageThisLap = GarageThisLap,
				LastSwapLap = LastSwapLap,
				LastSwapTime = LastSwapTime,
				SwapThisLap = SwapThisLap,
				SwapLocation = SwapLocation,
				TotalPenalties = TotalPenalties,
				TotalPitstops = TotalPitstops,
			};
		}
	}
}
