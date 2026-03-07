import { Injectable, InjectionToken, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom, Observable } from 'rxjs';
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
	private httpClient = inject(HttpClient);
	baseUrl = "api";

	constructor() { }

	getSessions(page: number, pageSize: number): Observable<IndexViewModel> {
		return this.httpClient.get<IndexViewModel>(this.baseUrl + '/Home/Index', { params: { page: page, pageSize: pageSize } });
	}

	getLiveSessions(): Observable<IndexViewModel> {
		return this.httpClient.get<IndexViewModel>(this.baseUrl + '/Home/LiveSessions');
	}

	getSession(sessionId: string): Promise<SessionViewModel> {
		return firstValueFrom(this.httpClient.get(this.baseUrl + '/Home/Session?sessionId=' + sessionId)) as Promise<SessionViewModel>;
	}

	getLaps(sessionId: string, carId: string): Promise<LapsViewModel> {
		return firstValueFrom(this.httpClient.get(this.baseUrl + '/Home/Laps?sessionId=' + sessionId + '&carId=' + carId)) as Promise<LapsViewModel>;
	}

	getEntryList(sessionId: string): Promise<Car[]> {
		return firstValueFrom(this.httpClient.get(this.baseUrl + '/Home/EntryList?sessionId=' + sessionId)) as Promise<Car[]>;
	}

	getTrackMap(sessionId: string): Promise<TrackMap> {
		return firstValueFrom(this.httpClient.get(this.baseUrl + '/Home/TrackMap?sessionId=' + sessionId)) as Promise<TrackMap>;
	}

	getChat(sessionId: string): Promise<ChatViewModel> {
		return firstValueFrom(this.httpClient.get(this.baseUrl + '/Home/Chat?sessionId=' + sessionId)) as Promise<ChatViewModel>;
	}

	getTracks(): Promise<string[]> {
		return firstValueFrom(this.httpClient.get(this.baseUrl + '/Home/Tracks')) as Promise<string[]>;
	}

	getBestLaps(filters: BestLapsFilters): Promise<BestLapsViewModel> {
		return firstValueFrom(this.httpClient.post(this.baseUrl + '/Home/BestLaps', filters)) as Promise<BestLapsViewModel>;
	}

	getAbout(): Observable<AboutOptions> {
		return this.httpClient.get<AboutOptions>(this.baseUrl + '/About');
	}
}

@Injectable({
	providedIn: 'root',
})
export class StaticServerApiService implements ServerApiService {
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
