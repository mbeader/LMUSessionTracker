import { Component, inject, viewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import { SessionService } from '../session.service';
import { Format } from '../../format';
import { SessionViewModel } from '../../view-models';
import { StandingsComponent } from '../standings/standings.component';
import { ResultsComponent } from '../results/results.component';

@Component({
	selector: 'app-sessions-session',
	imports: [RouterLink, StandingsComponent, ResultsComponent],
	providers: [SessionService],
	templateUrl: './session.component.html',
	styleUrl: './session.component.css',
})
export class SessionComponent {
	private service = inject(SessionService);
	private standings = viewChild(StandingsComponent);
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
