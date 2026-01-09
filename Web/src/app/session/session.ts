import { Component, inject, ChangeDetectorRef, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ServerApiService } from '../server-api.service';
import { ServerLiveService } from '../server-live.service';
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
	private live = inject(ServerLiveService);
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
			if (this.session?.session?.sessionId && this.hasStandings)
				this.live.joinLive(this.session.session.sessionId, this.updateSession.bind(this));
		}, error => { console.log(error); })
	}

	updateSession(session: SessionViewModel) {
		if (this.session) {
			SessionViewModel.merge(this.session, session);
			this.ref.markForCheck();
		}
	}

	ngOnInit() {

	}
}
