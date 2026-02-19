import { inject, ChangeDetectorRef, Injectable } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ServerApiService } from '../server-api.service';
import { ServerLiveService } from '../server-live.service';
import { SettingsService } from '../settings.service';
import { Anonymizer } from '../anonymizer.service';
import { SessionTransitionViewModel, SessionViewModel } from '../view-models';
import { Session as SessionModel } from '../models';

@Injectable()
export class SessionService {
	private shouldAnonymize = false;
	private ref = inject(ChangeDetectorRef);
	private route = inject(ActivatedRoute);
	private router = inject(Router);
	private api = inject(ServerApiService);
	private live = inject(ServerLiveService);
	private settings = inject(SettingsService);
	private anonymizer = inject(Anonymizer);
	private buildAutotransitionRoute: (sessionId: string) => string[] = () => [];
	private sessionId?: string;
	session: SessionViewModel | null = null;
	hasStandings: boolean = false;
	fahrenheit: boolean = true;

	init(onInit: (sessionId: string) => void, buildAutotransitionRoute: (sessionId: string) => string[]) {
		this.buildAutotransitionRoute = buildAutotransitionRoute;
		let sessionId = this.route.snapshot.paramMap.get('sessionId');
		if (!sessionId)
			return;
		this.fahrenheit = this.settings.get().fahrenheit === 'true';
		this.route.paramMap.subscribe(paramMap => {
			let newSessionId = paramMap.get('sessionId');
			if (!newSessionId)
				return;
			if (this.sessionId != newSessionId) {
				if (this.sessionId)
					this.live.leave();
				onInit(newSessionId);
			}
		});
	}

	getSession(sessionId: string, onUpdate: (session: SessionViewModel) => void, onFirst?: (session: SessionViewModel) => void) {
		this.api.getSession(sessionId).then(result => {
			if (this.shouldAnonymize)
				this.anonymize(result);
			this.session = result;
			this.hasStandings = this.session.standings != null && this.session.standings.length > 0;
			if (!this.hasStandings && this.session.session && new Date().valueOf() - new Date(this.session.session.timestamp + 'Z').valueOf() < 10000) {
				// actual session transitions are slow enough to notify live clients before any standings actually exist
				this.hasStandings = true;
				this.session.standings = [];
			}
			if (onFirst)
				onFirst(this.session);
			onUpdate(this.session);
			this.ref.markForCheck();
			if (this.session?.session?.sessionId && this.hasStandings) {
				this.live.joinLive(this.session.session.sessionId, this.updateSession.bind(this, onUpdate), this.transitionSession.bind(this));
			}
		}, error => { console.log(error); })
	}

	updateSession(onUpdate: (session: SessionViewModel) => void, session: SessionViewModel) {
		if (this.session) {
			if (this.shouldAnonymize)
				this.anonymize(session);
			SessionViewModel.merge(this.session, session);
			onUpdate(this.session);
			this.ref.markForCheck();
		}
	}

	transitionSession(session: SessionTransitionViewModel) {
		if (this.session && !this.session.nextSession && session.sessionId && session.info?.session) {
			if (this.settings.get().autotransition) {
				let route = this.buildAutotransitionRoute(session.sessionId);
				if (route.length > 0) {
					this.router.navigate(route);
					return;
				}
			}
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
