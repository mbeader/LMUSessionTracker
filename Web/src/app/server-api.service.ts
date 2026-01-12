import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { IndexViewModel, SessionViewModel, LapsViewModel } from './view-models';
import { Lap } from './models';
import { Entry } from './tracking';

@Injectable({
	providedIn: 'root',
})
export class ServerApiService {
	private httpClient = inject(HttpClient);
	baseUrl = "api";

	constructor() { }

	getSessions(): Promise<IndexViewModel> {
		return firstValueFrom(this.httpClient.get(this.baseUrl + '/Home/Index')) as Promise<IndexViewModel>;
	}

	getSession(sessionId: string): Promise<SessionViewModel> {
		return firstValueFrom(this.httpClient.get(this.baseUrl + '/Home/Session?sessionId=' + sessionId)) as Promise<SessionViewModel>;
	}

	getLaps(sessionId: string, carId: string): Promise<LapsViewModel> {
		return firstValueFrom(this.httpClient.get(this.baseUrl + '/Home/Laps?sessionId=' + sessionId + '&carId=' + carId)) as Promise<LapsViewModel>;
	}

	getEntryList(sessionId: string): Promise<Entry[]> {
		return firstValueFrom(this.httpClient.get(this.baseUrl + '/Home/EntryList?sessionId=' + sessionId)) as Promise<Entry[]>;
	}

	getTracks(): Promise<string[]> {
		return firstValueFrom(this.httpClient.get(this.baseUrl + '/Home/Tracks')) as Promise<string[]>;
	}

	getBestLaps(track: string, network: string, classes: string[]): Promise<Lap[]> {
		let params = new HttpParams().appendAll({ track: track, network: network, classes: classes });
		return firstValueFrom(this.httpClient.get(this.baseUrl + '/Home/BestLaps', { params })) as Promise<Lap[]>;
	}
}
