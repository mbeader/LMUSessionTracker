import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom, Observable } from 'rxjs';
import { IndexViewModel, SessionViewModel, LapsViewModel, BestLapsFilters } from './view-models';
import { Lap, Car } from './models';

@Injectable({
	providedIn: 'root',
})
export class ServerApiService {
	private httpClient = inject(HttpClient);
	baseUrl = "api";

	constructor() { }

	getSessions(page: number, pageSize: number): Observable<IndexViewModel> {
		return this.httpClient.get<IndexViewModel>(this.baseUrl + '/Home/Index', { params: { page: page, pageSize: pageSize } });
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

	getTracks(): Promise<string[]> {
		return firstValueFrom(this.httpClient.get(this.baseUrl + '/Home/Tracks')) as Promise<string[]>;
	}

	getBestLaps(filters: BestLapsFilters): Promise<Lap[]> {
		return firstValueFrom(this.httpClient.post(this.baseUrl + '/Home/BestLaps', filters)) as Promise<Lap[]>;
	}
}
