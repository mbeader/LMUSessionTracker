import { inject, Injectable } from '@angular/core';
import { NavigationStart, Router } from '@angular/router';
import { filter } from 'rxjs';
import { HubConnection, HubConnectionState } from '@microsoft/signalr';
import { ChatViewModel, JoinRequest, LapsViewModel, SessionTransitionViewModel, SessionViewModel } from './view-models';
import { SessionSummary } from './tracking';

declare var signalR: any;
@Injectable({
	providedIn: 'root',
})
export class ServerLiveService {
	private readonly retryDelay = [0, 2000, 5000, 10000, 30000, 60000, 120000, 300000, 600000, 1800000, 3600000];
	private readonly router = inject(Router);
	private connection: HubConnection | null = null;
	private stop: boolean = false;
	private retries: number = 0;

	constructor() {
		this.router.events.pipe(
			filter(e => e instanceof NavigationStart)
		).subscribe(async (route: NavigationStart) => {
			await this.reset();
		});
	}

	private async reset() {
		this.stop = true;
		this.retries = 0;
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

	joinChat(sessionId: string, callback: (chat: ChatViewModel) => void) {
		this.join(new JoinRequest('chat', sessionId, undefined, true), connection => {
			connection.on('Chat', (chat: ChatViewModel) => {
				callback(chat);
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
			let delay = this.retryDelay[this.retries < this.retryDelay.length ? this.retries : this.retryDelay.length - 1];
			this.retries++;
			console.debug(`Retry #${this.retries} in ${delay / 1000}s`);
			setTimeout(this.start.bind(this, req), delay);
		}
	};
}
