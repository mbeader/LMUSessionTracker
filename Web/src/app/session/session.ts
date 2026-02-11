import { Component, inject, ChangeDetectorRef, viewChild } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ServerApiService } from '../server-api.service';
import { ServerLiveService } from '../server-live.service';
import { Anonymizer } from '../anonymizer.service';
import { Format } from '../format';
import { SessionTransitionViewModel, SessionViewModel } from '../view-models';
import { Session as SessionModel } from '../models';
import { Standings } from './standings/standings';
import { Results } from './results/results';

@Component({
	selector: 'app-session',
	imports: [RouterLink, Standings, Results],
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
	private sessionId?: string;
	session: SessionViewModel | null = null;
	hasStandings: boolean = false;
	Format = Format;
	flagClass = SessionViewModel.flagClass;

	constructor() {
		let sessionId = this.route.snapshot.paramMap.get('sessionId');
		if (!sessionId)
			return;
		this.route.paramMap.subscribe(paramMap => {
			let newSessionId = paramMap.get('sessionId');
			if (!newSessionId)
				return;
			if (this.sessionId != newSessionId) {
				if (this.sessionId)
					this.live.leave();
				this.getSession(newSessionId);
			}
		});
	}

	getSession(sessionId: string) {
		this.api.getSession(sessionId).then(result => {
			if (this.shouldAnonymize)
				this.anonymize(result);
			this.session = result;
			this.hasStandings = this.session.standings != null && this.session.standings.length > 0;
			this.ref.markForCheck();
			if (this.session?.session?.sessionId && this.hasStandings)
				this.live.joinLive(this.session.session.sessionId, this.updateSession.bind(this), this.transitionSession.bind(this));
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

	transitionSession(session: SessionTransitionViewModel) {
		if (this.session && !this.session.nextSession && session.sessionId && session.info?.session) {
			this.session.nextSession = new Object() as SessionModel;
			this.session.nextSession.sessionId = session.sessionId;
			this.session.nextSession.sessionType = session.info?.session;
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
