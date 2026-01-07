import { Component, inject, ChangeDetectorRef } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ServerApiService } from '../server-api.service';

@Component({
	selector: 'app-sessions',
	imports: [RouterLink],
	templateUrl: './sessions.html',
	styleUrl: './sessions.css',
})
export class Sessions {
	private ref = inject(ChangeDetectorRef);
	private api = inject(ServerApiService);
	sessionsList: any[] = [];

	constructor() {
		this.api.getSessions().then(result => {
			this.sessionsList = result.sessions;
			this.ref.markForCheck();
		}, error => { console.log(error); })
	}
}
