import { Component, inject, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { TimingService } from '../timing.service';
import { SessionViewModel } from '../../view-models';
import { Best, Bests, Car, CarKey } from '../../tracking';
import { Standing } from '../../lmu';
import { Format } from '../../format';
import { classId } from '../../utils';
import { ClassBadge } from '../class-badge/class-badge';

@Component({
	selector: 'app-session-standings',
	imports: [RouterLink, ClassBadge],
	providers: [TimingService],
	templateUrl: './standings.html',
	styleUrl: './standings.css',
})
export class Standings {
	private timingService = inject(TimingService);
	@Input() session: SessionViewModel | null = null;
	Format = Format;
	Utils = { classId };
	CarKey = CarKey;

	ngOnChanges() {
		this.timingService.session = this.session;
		this.timingService.onChange();
	}

	get entries() { return this.timingService.entries; }
	get positionInClass() { return this.timingService.positionInClass; }
	get classBests() { return this.timingService.classBests; }
	get bests() { return this.timingService.bests; }
	get isRace() { return this.timingService.isRace; }

	getLastClasses(standing: Standing, carClass: string) { return this.timingService.getLastClasses(standing, carClass); }
	getBestClasses(standing: Standing, carClass: string) { return this.timingService.getBestClasses(standing, carClass); }
}
