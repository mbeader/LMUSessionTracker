import { inject, Injectable } from '@angular/core';
import { NavigationStart, Router } from '@angular/router';
import { filter } from 'rxjs';
import { HubConnection, HubConnectionState } from '@microsoft/signalr';
import { JoinRequest, LapsViewModel, SessionTransitionViewModel, SessionViewModel } from './view-models';
import { SessionSummary } from './tracking';

declare var signalR: any;
@Injectable({
	providedIn: 'root',
})
export class ServerLiveService {
	private readonly router = inject(Router);
	private connection: HubConnection | null = null;
	private stop: boolean = false;

	constructor() {
		this.router.events.pipe(
			filter(e => e instanceof NavigationStart)
		).subscribe(async (route: NavigationStart) => {
			await this.reset();
		});
	}

	private async reset() {
		this.stop = true;
		if (this.connection) {
			if (this.connection.state == HubConnectionState.Connected) {
				await this.connection.invoke('Leave');
				console.log('Left');
			}
			await this.connection.stop();
		}
	}

	private async join(req: JoinRequest, configure: (connection: HubConnection) => void) {
		await this.reset();

		this.connection = new signalR.HubConnectionBuilder()
			.withUrl('/api/Live/Session')
			.configureLogging(signalR.LogLevel.Information)
			.build() as HubConnection;

		this.connection.on('Joined', (req: JoinRequest) => {
			console.log('Joined', req.type, req.sessionId, req.key);
		});

		this.connection.on('Kicked', async () => {
			console.log('Kicked');
			await this.reset();
		});

		configure(this.connection);

		this.connection.onclose(async () => {
			await this.start(req);
		});

		this.stop = false;
		await this.start(req);
	}

	joinSessions(callback: (sessions: SessionSummary[]) => void) {
		this.join(new JoinRequest('sessions'), connection => {
			connection.on('Sessions', (sessions: SessionSummary[]) => {
				callback(sessions);
			});
		});
	}

	joinLive(sessionId: string, callback: (session: SessionViewModel) => void, transitionCallback: (session: SessionTransitionViewModel) => void) {
		this.join(new JoinRequest('live', sessionId), connection => {
			connection.on('Live', (session: SessionViewModel) => {
				callback(session);
			});
			connection.on('Transition', (session: SessionTransitionViewModel) => {
				transitionCallback(session);
			});
		});
	}

	joinLaps(sessionId: string, carId: string, callback: (laps: LapsViewModel) => void) {
		this.join(new JoinRequest('laps', sessionId, carId), connection => {
			connection.on('Laps', (laps: LapsViewModel) => {
				callback(laps);
			});
		});
	}

	leave() {
		this.reset();
	}

	private async start(req: JoinRequest) {
		if (this.stop || !this.connection || this.connection.state != HubConnectionState.Disconnected)
			return;
		try {
			await this.connection.start();
			console.log('SignalR Connected.');
			await this.connection.invoke('Join', req);
		} catch (err) {
			console.log(err);
			setTimeout(this.start.bind(this, req), 5000);
		}
	};
}
