namespace LMUSessionTracker.Core.Services {
	public interface ContinueProvider<T> {
		public bool ShouldContinue();
	}

	public interface ContinueProviderSource {
		public bool Continue { get; set; }
	}

	public class SimpleContinueProvider<T> : ContinueProviderSource, ContinueProvider<T> {
		public bool Continue { get; set; } = true;

		public bool ShouldContinue() => Continue;
	}
}
