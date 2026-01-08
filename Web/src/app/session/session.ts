import { Component, inject, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ServerApiService } from '../server-api.service';
import { Format } from '../format';
import { SessionViewModel } from '../view-models';
import { Standings } from './standings/standings';

@Component({
	selector: 'app-session',
	imports: [RouterLink, Standings],
	templateUrl: './session.html',
	styleUrl: './session.css',
})
export class Session {
	private ref = inject(ChangeDetectorRef);
	private route = inject(ActivatedRoute);
	private api = inject(ServerApiService);
	session: SessionViewModel | null = null;
	hasStandings: boolean = false;
	Format = Format;

	constructor() {
		let sessionId = this.route.snapshot.paramMap.get('sessionId');
		if (!sessionId)
			return;
		this.api.getSession(sessionId).then(result => {
			this.session = result;
			this.hasStandings = this.session.standings != null && this.session.standings.length > 0;
			this.ref.markForCheck();
		}, error => { console.log(error); })
	}
}
