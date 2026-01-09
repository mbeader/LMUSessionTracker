import { inject, Injectable } from '@angular/core';
import { NavigationStart, Router } from '@angular/router';
import { filter } from 'rxjs';
import { HubConnection, HubConnectionState } from '@microsoft/signalr';
import { SessionViewModel } from './view-models';

declare var signalR: any;
@Injectable({
	providedIn: 'root',
})
export class ServerLiveService {
	private readonly router = inject(Router);
	private connection: HubConnection | null = null;

	constructor() {
		this.router.events.pipe(
			filter(e => e instanceof NavigationStart)
		).subscribe(async (route: NavigationStart) => {
			if (this.connection) {
				await this.connection.invoke('Leave');
				console.log('Left');
			}
		});
	}

	private join(sessionId: string, type: string, configure: (connection: HubConnection) => void) {
		this.connection = new signalR.HubConnectionBuilder()
			.withUrl('/api/Live/Session')
			.configureLogging(signalR.LogLevel.Information)
			.build() as HubConnection;

		this.connection.on('Joined', (sessionId: string, type: string) => {
			console.log('Joined', sessionId, type);
		});

		configure(this.connection);

		this.connection.onclose(async () => {
			await this.start(sessionId, type);
		});

		this.start(sessionId, type);
	}

	joinLive(sessionId: string, callback: (session: SessionViewModel) => void) {
		this.join(sessionId, 'live', connection => {
			connection.on('Live', (session: SessionViewModel) => {
				callback(session);
			});
		});
	}

	private async start(sessionId: string, type: string) {
		if (!this.connection || this.connection.state != HubConnectionState.Disconnected)
			return;
		try {
			await this.connection.start();
			console.log('SignalR Connected.');
			await this.connection.invoke('Join', sessionId, type);
		} catch (err) {
			console.log(err);
			setTimeout(this.start.bind(this, sessionId, type), 5000);
		}
	};
}
