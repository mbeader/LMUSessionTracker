using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Test {
	internal class Program {
		static void Main(string[] args) {
			string baseurl = "http://localhost:6397";
			HttpClient client = new HttpClient();
			Run(baseurl, client).Wait();
		}

		private static async Task Run(string baseurl, HttpClient client) {
			List<string> paths = new List<string>() {
				//"/rest/chat/",
				//"/rest/garage/getPlayerGarageData/",
				//"/rest/garage/getVehicleCondition/",
				//"/rest/garage/setup/",
				//"/rest/hud/",
				//"/rest/multiplayer/join/",
				//"/rest/multiplayer/join/state",
				//"/rest/multiplayer/teams/",
				//"/rest/race/car",
				//"/rest/race/track",
				//"/rest/sessions/?",
				//"/rest/sessions/GetGameState",
				//"/rest/sessions/GetSessionsInfoForEvent",
				//"/rest/sessions/amount",
				//"/rest/sessions/getAllVehicles",
				//"/rest/sessions/getTracksInSeries",
				//"/rest/sessions/opponents",
				//"/rest/sessions/opponents/all",
				//"/rest/sessions/weather",
				//"/rest/strategy/overall",
				//"/rest/strategy/pitstop-estimate",
				//"/rest/strategy/usage",
				"/rest/watch/sessionInfo",
				"/rest/watch/standings",
				"/rest/watch/standings/history",
				"/rest/watch/trackmap",
				"/webdata",
			};
			foreach(string path in paths)
				await MakeRequest(baseurl, client, path);
		}

		private static async Task MakeRequest(string baseurl, HttpClient client, string path) {
			string url = baseurl + path;
			try {
				HttpResponseMessage res = await client.GetAsync(url);
				if(res.StatusCode == System.Net.HttpStatusCode.OK && res.Content != null) {
					File.WriteAllText(DateTime.UtcNow.ToString("yyyyMMddHHmmssfff") + path.Replace("/", "_") + ".json", await res.Content.ReadAsStringAsync());
				}
			} catch(Exception e) {
				Console.WriteLine($"Request failed: {url}\r\n{e}");
			}
		}
	}
}
