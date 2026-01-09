import { inject, Injectable } from '@angular/core';
import { NavigationStart, Router } from '@angular/router';
import { filter } from 'rxjs';
import { SessionInfo } from './lmu';

declare var signalR: any;
@Injectable({
	providedIn: 'root',
})
export class ServerLiveService {
	private readonly router = inject(Router);
	private connection: any;

	constructor() {
		this.router.events.pipe(
			filter(e => e instanceof NavigationStart)
		).subscribe(async (route: NavigationStart) => {
			if (this.connection && this.connection.invoke)
				await this.connection.invoke('Leave');
		});
	}

	join(sessionId: string, type: string) {
		const connection = new signalR.HubConnectionBuilder()
			.withUrl('/api/Live/Session')
			.configureLogging(signalR.LogLevel.Information)
			.build();

		async function start() {
			try {
				await connection.start();
				console.log('SignalR Connected.');
				await connection.invoke('Join', sessionId, type);
			} catch (err) {
				console.log(err);
				setTimeout(start, 5000);
			}
		};

		connection.on('Joined', (sessionId: string, type: string) => {
			console.log('Joined', sessionId, type);
		});

		if (type == 'live') {
			connection.on('SessionInfo', (session: SessionInfo) => {
				console.log('SessionInfo', session);
			});
		}

		connection.onclose(async () => {
			await start();
		});

		this.connection = connection;
		start();
	}
}
