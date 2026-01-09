using LMUSessionTracker.Core.Json;
using LMUSessionTracker.Core.LMU;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace LMUSessionTracker.Core.Tests.Json {
	public class SystemTextJsonSchemaValidatorTests {
		private readonly ILogger<SystemTextJsonSchemaValidator> logger;

		static SystemTextJsonSchemaValidatorTests() {
			string dir = Path.Join("logs", "schema");
			Directory.CreateDirectory(dir);
			foreach(string file in Directory.EnumerateFiles(dir))
				File.Delete(file);
			SystemTextJsonSchemaValidator.LoadJsonSchemas();
		}

		public SystemTextJsonSchemaValidatorTests(LoggingFixture loggingFixture) {
			logger = loggingFixture.LoggerFactory.CreateLogger<SystemTextJsonSchemaValidator>();
		}

		[Fact]
		public void LoadJsonSchemas_AlreadyLoaded_ThrowsException() {
			try {
				SystemTextJsonSchemaValidator.LoadJsonSchemas();
				Assert.Fail("Schemas were not loaded");
			} catch(Exception e) {
				Assert.Equal("Schemas already loaded", e.Message);
			}
		}

		public class Test {
			public string A { get; set; }
		}

		//[Fact]
		//public void GenerateJsonSchema_WritesSchema() {
		//	string filename = "schema-Test.json";
		//	File.Delete(filename);
		//	SystemTextJsonSchemaValidator.GenerateJsonSchema(new() { typeof(Test) });
		//	Assert.True(File.Exists(filename));
		//	File.Delete(filename);
		//}

		[Fact]
		public void Validate_Chat_InvalidJson_ReturnsFalse() {
			Assert.False(SystemTextJsonSchemaValidator.Validate("\\", typeof(Chat[]), null, logger));
		}

		[Fact]
		public void Validate_Chat_InvalidType_ReturnsFalse() {
			Assert.False(SystemTextJsonSchemaValidator.Validate("\"t\"", typeof(Chat[]), null, logger));
		}

		[Fact]
		public void Validate_Chat_InvalidProperty_ReturnsFalse() {
			Assert.False(SystemTextJsonSchemaValidator.Validate("[{\"message\":\"t\",\"timestamp\":1,\"x\":0}]", typeof(Chat[]), null, logger));
		}

		[Fact]
		public void Validate_Chat_InvalidPropertyType_ReturnsFalse() {
			Assert.False(SystemTextJsonSchemaValidator.Validate("[{\"message\":0,\"timestamp\":1}]", typeof(Chat[]), null, logger));
		}

		[Fact]
		public void Validate_Chat_InvalidPropertyConstraint_ReturnsFalse() {
			Assert.False(SystemTextJsonSchemaValidator.Validate("[{\"message\":\"t\",\"timestamp\":-1}]", typeof(Chat[]), null, logger));
		}

		[Fact]
		public void Validate_Chat_Valid_ReturnsTrue() {
			Assert.True(SystemTextJsonSchemaValidator.Validate("[{\"message\":\"t\",\"timestamp\":1}]", typeof(Chat[]), null, logger));
		}
	}
}
