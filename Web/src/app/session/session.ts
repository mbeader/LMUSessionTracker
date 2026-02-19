import { Component, inject, viewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import { SessionService } from './session.service';
import { Format } from '../format';
import { SessionViewModel } from '../view-models';
import { Standings } from './standings/standings';
import { Results } from './results/results';

@Component({
	selector: 'app-session',
	imports: [RouterLink, Standings, Results],
	providers: [SessionService],
	templateUrl: './session.html',
	styleUrl: './session.css',
})
export class Session {
	private service = inject(SessionService);
	private standings = viewChild(Standings);
	Format = Format;
	flagClass = SessionViewModel.flagClass;

	get session() { return this.service.session; }
	get hasStandings() { return this.service.hasStandings; }
	get fahrenheit() { return this.service.fahrenheit; }

	constructor() {
		this.service.init(this.onInitSession.bind(this), sessionId => ['/', 'Session', sessionId]);
	}

	private onInitSession(sessionId: string) {
		this.service.getSession(sessionId, this.onUpdateSession.bind(this));
	}

	private onUpdateSession(session: SessionViewModel) {
		this.standings()?.ngOnChanges();
	}
}
