import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';
import { IndexViewModel, SessionViewModel, LapsViewModel } from './view-models';

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
}
