import { Component, inject, ChangeDetectorRef } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ServerApiService } from '../server-api.service';
import { Format } from '../format';
import { SessionSummary } from '../tracking';

@Component({
	selector: 'app-sessions',
	imports: [RouterLink],
	templateUrl: './sessions.html',
	styleUrl: './sessions.css',
})
export class Sessions {
	private ref = inject(ChangeDetectorRef);
	private api = inject(ServerApiService);
	sessions: SessionSummary[] = [];
	now: Date = new Date();
	Format = Format;

	constructor() {
		this.api.getSessions().then(result => {
			this.sessions = result.sessions ?? [];
			this.ref.markForCheck();
		}, error => { console.log(error); })
	}
}
