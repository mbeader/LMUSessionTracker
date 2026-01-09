using LMUSessionTracker.Core.LMU;
using LMUSessionTracker.Server.Json;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace LMUSessionTracker.Server.Tests.Json {
	public class NewtonsoftSchemaValidatorTests {
		private readonly ILogger<NewtonsoftSchemaValidator> logger;

		static NewtonsoftSchemaValidatorTests() {
			string dir = Path.Join("logs", "schema");
			Directory.CreateDirectory(dir);
			foreach(string file in Directory.EnumerateFiles(dir))
				File.Delete(file);
			NewtonsoftSchemaValidator.LoadJsonSchemas();
		}

		public NewtonsoftSchemaValidatorTests(LoggingFixture loggingFixture) {
			logger = loggingFixture.LoggerFactory.CreateLogger<NewtonsoftSchemaValidator>();
		}

		[Fact]
		public void LoadJsonSchemas_AlreadyLoaded_ThrowsException() {
			try {
				NewtonsoftSchemaValidator.LoadJsonSchemas();
				Assert.Fail("Schemas were not loaded");
			} catch(Exception e) {
				Assert.Equal("Schemas already loaded", e.Message);
			}
		}

		public class Test {
			public string A { get; set; }
		}

		[Fact]
		public void GenerateJsonSchema_WritesSchema() {
			string filename = "schema-Test.json";
			File.Delete(filename);
			NewtonsoftSchemaValidator.GenerateJsonSchema(new() { typeof(Test) });
			Assert.True(File.Exists(filename));
			File.Delete(filename);
		}

		[Fact]
		public void Validate_Chat_InvalidJson_ReturnsFalse() {
			Assert.False(NewtonsoftSchemaValidator.Validate("\\", typeof(Chat[]), null, logger));
		}

		[Fact]
		public void Validate_Chat_InvalidType_ReturnsFalse() {
			Assert.False(NewtonsoftSchemaValidator.Validate("\"t\"", typeof(Chat[]), null, logger));
		}

		[Fact]
		public void Validate_Chat_InvalidProperty_ReturnsFalse() {
			Assert.False(NewtonsoftSchemaValidator.Validate("[{\"message\":\"t\",\"timestamp\":1,\"x\":0}]", typeof(Chat[]), null, logger));
		}

		[Fact]
		public void Validate_Chat_InvalidPropertyType_ReturnsFalse() {
			Assert.False(NewtonsoftSchemaValidator.Validate("[{\"message\":0,\"timestamp\":1}]", typeof(Chat[]), null, logger));
		}

		[Fact]
		public void Validate_Chat_InvalidPropertyConstraint_ReturnsFalse() {
			Assert.False(NewtonsoftSchemaValidator.Validate("[{\"message\":\"t\",\"timestamp\":-1}]", typeof(Chat[]), null, logger));
		}

		[Fact]
		public void Validate_Chat_Valid_ReturnsTrue() {
			Assert.True(NewtonsoftSchemaValidator.Validate("[{\"message\":\"t\",\"timestamp\":1}]", typeof(Chat[]), null, logger));
		}
	}
}
