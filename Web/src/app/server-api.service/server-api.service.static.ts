import { Injectable, InjectionToken } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { IndexViewModel, SessionViewModel, LapsViewModel, BestLapsFilters, BestLapsViewModel, AboutOptions, TrackMap, ChatViewModel } from '../view-models';
import { Car } from '../models';

export const ServerApiServiceToken = new InjectionToken<ServerApiService>('server api');

export interface ServerApiService {
	getSessions(page: number, pageSize: number): Observable<IndexViewModel>;
	getLiveSessions(): Observable<IndexViewModel>;
	getSession(sessionId: string): Promise<SessionViewModel>;
	getLaps(sessionId: string, carId: string): Promise<LapsViewModel>;
	getEntryList(sessionId: string): Promise<Car[]>;
	getTrackMap(sessionId: string): Promise<TrackMap>;
	getChat(sessionId: string): Promise<ChatViewModel>;
	getTracks(): Promise<string[]>;
	getBestLaps(filters: BestLapsFilters): Promise<BestLapsViewModel>;
	getAbout(): Observable<AboutOptions>;
}

@Injectable({
	providedIn: 'root',
})
export class HttpServerApiService implements ServerApiService {
    getSessions(page: number, pageSize: number): Observable<IndexViewModel> {
        throw new Error('Method not implemented.');
    }
    getLiveSessions(): Observable<IndexViewModel> {
        throw new Error('Method not implemented.');
    }
    getSession(sessionId: string): Promise<SessionViewModel> {
        throw new Error('Method not implemented.');
    }
    getLaps(sessionId: string, carId: string): Promise<LapsViewModel> {
        throw new Error('Method not implemented.');
    }
    getEntryList(sessionId: string): Promise<Car[]> {
        throw new Error('Method not implemented.');
    }
    getTrackMap(sessionId: string): Promise<TrackMap> {
        throw new Error('Method not implemented.');
    }
    getChat(sessionId: string): Promise<ChatViewModel> {
        throw new Error('Method not implemented.');
    }
    getTracks(): Promise<string[]> {
        throw new Error('Method not implemented.');
    }
    getBestLaps(filters: BestLapsFilters): Promise<BestLapsViewModel> {
        throw new Error('Method not implemented.');
    }
    getAbout(): Observable<AboutOptions> {
        throw new Error('Method not implemented.');
    }
}

@Injectable({
	providedIn: 'root',
})
export class StaticServerApiService implements ServerApiService {
	private delay(callback: () => void) {
		setTimeout(callback, 100);
	}

	getSessions(page: number, pageSize: number): Observable<IndexViewModel> {
		return new Observable(subscriber => this.delay(() => subscriber.next(Object.assign(new IndexViewModel(), { sessions: [] }))));
	}

	getLiveSessions(): Observable<IndexViewModel> {
		return new Observable(subscriber => this.delay(() => subscriber.next(Object.assign(new IndexViewModel(), { sessions: [] }))));
	}

	getSession(sessionId: string): Promise<SessionViewModel> {
		return new Promise(resolve => resolve(new SessionViewModel()));
	}

	getLaps(sessionId: string, carId: string): Promise<LapsViewModel> {
		return new Promise(resolve => resolve(new LapsViewModel()));
	}

	getEntryList(sessionId: string): Promise<Car[]> {
		return new Promise(resolve => resolve([]));
	}

	getTrackMap(sessionId: string): Promise<TrackMap> {
		return new Promise(resolve => resolve(new TrackMap()));
	}

	getChat(sessionId: string): Promise<ChatViewModel> {
		return new Promise(resolve => resolve(new ChatViewModel()));
	}

	getTracks(): Promise<string[]> {
		return new Promise(resolve => resolve([]));
	}

	getBestLaps(filters: BestLapsFilters): Promise<BestLapsViewModel> {
		return new Promise(resolve => resolve(new BestLapsViewModel()));
	}

	getAbout(): Observable<AboutOptions> {
		return new Observable(subscriber => { subscriber.next(Object.assign(new AboutOptions(), { repoUrl: environment.repoUrl }) ); });
	}
}
