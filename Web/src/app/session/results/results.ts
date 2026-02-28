import { ChangeDetectorRef, Component, inject, Input, viewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ServerApiService } from '../../server-api.service';
import { SessionViewModel } from '../../view-models';
import { Car, CarKey } from '../../tracking';
import { Format } from '../../format';
import { classId, statusClass, whenExists } from '../../utils';
import { Lap } from '../../models';
import { ClassBadge } from '../class-badge/class-badge';
import { PitSummary } from '../pit-summary/pit-summary';
import { CarStatusDescription, TimingService } from '../timing.service';
import { Standing } from '../../lmu';

@Component({
	selector: 'app-session-results',
	imports: [RouterLink, ClassBadge, PitSummary],
	templateUrl: './results.html',
	styleUrl: './results.css',
})
export class Results {
	private ref = inject(ChangeDetectorRef);
	private api = inject(ServerApiService);
	private pitSummary = viewChild(PitSummary);
	@Input() session: SessionViewModel | null = null;
	positionInClass: Map<string, number> = new Map();
	entries: Map<string, Car> = new Map();
	gaps: { behind: number, interval: number }[] = [];
	results: Lap[] = [];
	isRace: boolean = false;
	carId?: string;
	carStatusDesc?: CarStatusDescription;
	Format = Format;
	Utils = { classId, statusClass };
	CarKey = CarKey;

	constructor() {
		whenExists('#pitModal', el => el.addEventListener('show.bs.modal', this.updatePitModal.bind(this)));
	}

	ngOnChanges() {
		this.isRace = this.session?.session?.sessionType.startsWith('RACE') ?? false;
		if (this.session?.session?.sessionId) {
			this.results = this.session.results ?? [];
			this.entries.clear();
			if (this.session && this.session.entries) {
				for (let car in this.session.entries) {
					this.entries.set(car ?? '', this.session.entries[car]);
				}
			}
			this.positionInClass.clear();
			let classCount = new Map<string, number>();
			let classTimes = new Map<string, { leader: number, last: number }>();
			for (let lap of this.results) {
				let key = CarKey.fromCar(lap.car);
				let pic = (classCount.get(lap.car.class) ?? 0) + 1;
				classCount.set(lap.car.class, pic);
				this.positionInClass.set(key.id, pic);

				let times = (classTimes.get(lap.car.class) ?? { leader: -1, last: -1 });
				if (times.leader < 0 || lap.totalTime <= 0)
					this.gaps.push({ behind: -1, interval: -1 });
				else
					this.gaps.push({ behind: lap.totalTime - times.leader, interval: lap.totalTime - times.last });
				if (times.leader < 0)
					classTimes.set(lap.car.class, { leader: lap.totalTime, last: lap.totalTime });
				else
					times.last = lap.totalTime;
			}
			this.ref.markForCheck();
		}
	}

	updatePitModal(e: any) {
		if (e && e.relatedTarget && e.relatedTarget instanceof HTMLButtonElement) {
			let button: HTMLButtonElement = e.relatedTarget;
			if (button.getAttribute('data-bs-target') != '#pitModal')
				return;
			let id = button.closest('tr')?.getAttribute('car-id');
			let car = this.entries.get(id ?? '');
			let result = this.results.find(x => CarKey.fromCar(x.car).id == id);
			if (id && car && result) {
				this.carId = id;
				this.pitSummary()?.setCar(id);
				this.carStatusDesc = TimingService.getCarDescription(car, { driverName: result.driver } as Standing);
			} else {
				this.pitSummary()?.clear();
				this.carStatusDesc = undefined;
			}
		}
	}
}
