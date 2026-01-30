import { Component, inject, ChangeDetectorRef, viewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ServerApiService } from '../server-api.service';
import { ServerLiveService } from '../server-live.service';
import { Anonymizer } from '../anonymizer.service';
import { Format } from '../format';
import { SessionViewModel } from '../view-models';
import { Standings } from './standings/standings';
import { Results } from './results/results';

@Component({
	selector: 'app-session',
	imports: [Standings, Results],
	templateUrl: './session.html',
	styleUrl: './session.css',
})
export class Session {
	private shouldAnonymize = false;
	private ref = inject(ChangeDetectorRef);
	private route = inject(ActivatedRoute);
	private api = inject(ServerApiService);
	private live = inject(ServerLiveService);
	private anonymizer = inject(Anonymizer);
	private standings = viewChild(Standings);
	session: SessionViewModel | null = null;
	hasStandings: boolean = false;
	Format = Format;
	flagClass = SessionViewModel.flagClass;

	constructor() {
		let sessionId = this.route.snapshot.paramMap.get('sessionId');
		if (!sessionId)
			return;
		this.api.getSession(sessionId).then(result => {
			if (this.shouldAnonymize)
				this.anonymize(result);
			this.session = result;
			this.hasStandings = this.session.standings != null && this.session.standings.length > 0;
			this.ref.markForCheck();
			if (this.session?.session?.sessionId && this.hasStandings)
				this.live.joinLive(this.session.session.sessionId, this.updateSession.bind(this));
		}, error => { console.log(error); })
	}

	updateSession(session: SessionViewModel) {
		if (this.session) {
			if (this.shouldAnonymize)
				this.anonymize(session);
			SessionViewModel.merge(this.session, session);
			this.standings()?.ngOnChanges();
			this.ref.markForCheck();
		}
	}

	anonymize(session: SessionViewModel) {
		if (session?.standings != null) {
			for (let standing of session.standings) {
				standing.driverName = this.anonymizer.driver(standing.driverName);
				standing.fullTeamName = standing.driverName;
			}
			for (let entry in session.entries) {
				let car = session.entries[entry];
				if (car)
					car.teamName = this.anonymizer.team(car.number, car.class);
			}
		}
	}
}
