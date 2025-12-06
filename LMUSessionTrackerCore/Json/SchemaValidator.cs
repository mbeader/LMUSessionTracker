using System;

namespace LMUSessionTracker.Core.Json {
	public interface SchemaValidator {
		public bool Validate(string json, Type type) ;
	}
}
