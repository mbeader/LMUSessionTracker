import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { firstValueFrom } from 'rxjs';

@Injectable({
	providedIn: 'root',
})
export class ServerApiService {
	private httpClient = inject(HttpClient);
	baseUrl = "api";

	constructor() { }

	getSessions(): Promise<any> {
		return firstValueFrom(this.httpClient.get(this.baseUrl + '/Home/Index'));
	}

	getSession(sessionId: string): Promise<any> {
		return firstValueFrom(this.httpClient.get(this.baseUrl + '/Home/Session?sessionId=' + sessionId));
	}

	getLaps(sessionId: string, carId: string): Promise<any> {
		return firstValueFrom(this.httpClient.get(this.baseUrl + '/Home/Laps?sessionId=' + sessionId + '&carId=' + carId));
	}
}
