using System;

namespace LMUSessionTracker.Common.Json {
	public interface SchemaValidator {
		public bool Validate(string json, Type type, string id = null);
	}
}
