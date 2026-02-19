import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { SessionService } from '../session.service';
import { TimingService } from '../timing.service';
import { Format } from '../../format';
import { SessionViewModel } from '../../view-models';
import { Standing } from '../../lmu';
import { CarKey } from '../../tracking';
import { classId, whenExists } from '../../utils';
import { ClassBadge } from '../class-badge/class-badge';

@Component({
	selector: 'app-session-timing',
	imports: [RouterLink, ClassBadge],
	providers: [SessionService, TimingService],
	templateUrl: './timing.html',
	styleUrl: './timing.css',
})
export class Timing {
	private service = inject(SessionService);
	private timingService = inject(TimingService);
	Utils = { classId };
	CarKey = CarKey;
	Format = Format;
	flagClass = SessionViewModel.flagClass;

	get session() { return this.service.session; }
	get hasStandings() { return this.service.hasStandings; }
	get fahrenheit() { return this.service.fahrenheit; }

	constructor() {
		this.service.init(this.onInitSession.bind(this), sessionId => ['/', 'Session', sessionId, 'Timing']);
		whenExists('main', main => { if(main.parentElement) main.parentElement.className = 'container-fluid'; });
	}

	private onInitSession(sessionId: string) {
		this.service.getSession(sessionId, this.onUpdateSession.bind(this));
	}

	private onUpdateSession(session: SessionViewModel) {
		this.timingService.session = this.session;
		this.timingService.onChange();
	}

	get entries() { return this.timingService.entries; }
	get carState() { return this.timingService.carState; }
	get positionInClass() { return this.timingService.positionInClass; }
	get classBests() { return this.timingService.classBests; }
	get bests() { return this.timingService.bests; }
	get isRace() { return this.timingService.isRace; }
	
	getLastClasses(standing: Standing, carClass: string) { return this.timingService.getLastClasses(standing, carClass); }
	getBestClasses(standing: Standing, carClass: string) { return this.timingService.getBestClasses(standing, carClass); }
}
