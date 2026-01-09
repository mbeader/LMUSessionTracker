import { Injectable } from '@angular/core';

declare var signalR: any;
@Injectable({
	providedIn: 'root',
})
export class ServerLiveService {
	join() {
		const connection = new signalR.HubConnectionBuilder()
			.withUrl("/api/Live/Session")
			.configureLogging(signalR.LogLevel.Information)
			.build();

		async function start() {
			try {
				await connection.start();
				console.log("SignalR Connected.");
				await connection.invoke("SendMessage", 'u', 'msg');
			} catch (err) {
				console.log(err);
				setTimeout(start, 5000);
			}
		};

		connection.on("ReceiveMessage", (user: any, message: any) => {
			console.log(user, message);
		});

		connection.onclose(async () => {
			await start();
		});

		// Start the connection.
		start();
	}
}
