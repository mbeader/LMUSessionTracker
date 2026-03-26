using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace LMUSessionTracker.Common.Replay {
	/// <summary>
	/// Loads everything at once, uses a ton of memory
	/// </summary>
	public class ReplayLoader {
		private static readonly int batchSize = 100;
		private readonly Dictionary<string, Task<Dictionary<string, string>>> results = new Dictionary<string, Task<Dictionary<string, string>>>();
		private readonly JsonSerializerOptions serializerOptions;

		public ReplayLoader(JsonSerializerOptions serializerOptions) {
			this.serializerOptions = serializerOptions;
		}

		public async Task Load(Queue<string> queue) {
			List<KeyValuePair<string, Task<Dictionary<string, string>>>> list = new List<KeyValuePair<string, Task<Dictionary<string, string>>>>();
			List<Task<Dictionary<string, string>>> tasks = new List<Task<Dictionary<string, string>>>();
			foreach(string s in queue) {
				Task<Dictionary<string, string>> task = Task.Run(() => ReadFile(s));
				list.Add(new(s, task));
				tasks.Add(task);
				if(tasks.Count == batchSize) {
					await Task.WhenAll(tasks);
					foreach(var item in list)
						results.Add(item.Key, item.Value);
					list.Clear();
					tasks.Clear();
				}
			}
			await Task.WhenAll(tasks);
			foreach(var item in list)
				results.Add(item.Key, item.Value);
		}

		public Task<Dictionary<string, string>> Read(string filename) {
			return results[filename];
		}

		private async Task<Dictionary<string, string>> ReadFile(string filename) {
			using(FileStream stream = File.OpenRead(filename)) {
				return (Dictionary<string, string>)await JsonSerializer.DeserializeAsync(stream, serializerOptions.GetTypeInfo(typeof(Dictionary<string, string>)));
			}
		}
	}
}
