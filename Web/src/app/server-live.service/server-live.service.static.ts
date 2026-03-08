import { Injectable, InjectionToken } from '@angular/core';
import { ChatViewModel, LapsViewModel, SessionTransitionViewModel, SessionViewModel } from '../view-models';
import { SessionSummary } from '../tracking';

export const ServerLiveServiceToken = new InjectionToken<ServerLiveService>('server live');

export interface ServerLiveService {
	joinSessions(callback: (sessions: SessionSummary[]) => void): void;
	joinLive(sessionId: string, callback: (session: SessionViewModel) => void, transitionCallback: (session: SessionTransitionViewModel) => void): void;
	joinLaps(sessionId: string, carId: string, callback: (laps: LapsViewModel) => void): void;
	joinChat(sessionId: string, callback: (chat: ChatViewModel) => void): void;
	leave(): void;
}

declare var signalR: any;
@Injectable({
	providedIn: 'root',
})
export class HttpServerLiveService implements ServerLiveService {
	joinSessions(callback: (sessions: SessionSummary[]) => void): void {
		throw new Error('Method not implemented.');
	}
	joinLive(sessionId: string, callback: (session: SessionViewModel) => void, transitionCallback: (session: SessionTransitionViewModel) => void): void {
		throw new Error('Method not implemented.');
	}
	joinLaps(sessionId: string, carId: string, callback: (laps: LapsViewModel) => void): void {
		throw new Error('Method not implemented.');
	}
	joinChat(sessionId: string, callback: (chat: ChatViewModel) => void): void {
		throw new Error('Method not implemented.');
	}
	leave(): void {
		throw new Error('Method not implemented.');
	}
}

export class StaticServerLiveService implements ServerLiveService {
	joinSessions(callback: (sessions: SessionSummary[]) => void) {
		callback([]);
	}

	joinLive(sessionId: string, callback: (session: SessionViewModel) => void, transitionCallback: (session: SessionTransitionViewModel) => void) {
		callback(new SessionViewModel());
	}

	joinLaps(sessionId: string, carId: string, callback: (laps: LapsViewModel) => void) {
		callback(new LapsViewModel());
	}

	joinChat(sessionId: string, callback: (chat: ChatViewModel) => void) {
		let vm = new ChatViewModel();
		vm.append = true;
		callback(vm);
	}

	leave() {
	}
}
