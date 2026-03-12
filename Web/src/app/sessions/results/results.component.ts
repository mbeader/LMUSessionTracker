import { ChangeDetectorRef, Component, inject, Input, viewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ServerApiService, ServerApiServiceToken } from '../../data/server-api/server-api.service';
import { BestClasses, CarStatusDescription, TimingService } from '../timing.service';
import { Result, SessionViewModel } from '../../view-models';
import { Best, Car, CarKey, Lap } from '../../tracking';
import { Standing } from '../../lmu';
import { Format } from '../../format';
import { classId, statusClass, whenExists } from '../../utils';
import { ClassBadgeComponent } from '../../cars/class-badge/class-badge.component';
import { BrandBadgeComponent } from '../../cars/brand-badge/brand-badge.component';
import { PitSummaryComponent } from '../pit-summary/pit-summary.component';

@Component({
	selector: 'app-sessions-results',
	imports: [RouterLink, ClassBadgeComponent, BrandBadgeComponent, PitSummaryComponent],
	templateUrl: './results.component.html',
	styleUrl: './results.component.css',
})
export class ResultsComponent {
	private ref = inject(ChangeDetectorRef);
	private api = inject(ServerApiServiceToken);
	private pitSummary = viewChild(PitSummaryComponent);
	@Input() session: SessionViewModel | null = null;
	positionInClass: Map<string, number> = new Map();
	entries: Map<string, Car> = new Map();
	gaps: { behind: number, interval: number, behindLaps: number, intervalLaps: number }[] = [];
	results: Result[] = [];
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
			if (this.session && this.session.positionInClass) {
				for (let car in this.session.positionInClass) {
					this.positionInClass.set(car ?? '', this.session.positionInClass[car]);
				}
			}
			if (this.isRace)
				this.calcGaps();
			else
				this.calcTimedGaps();
			this.ref.markForCheck();
		}
	}

	private calcGaps() {
		this.gaps.length = 0;
		let leadInClass = new Map<string, Result>();
		let prevInClass = new Map<string, Result>();
		for (let i = 0; i < this.results.length; i++) {
			let result = this.results[i];
			let lead = leadInClass.get(result.car.class);
			if (!lead || !result.carState || !lead.carState) {
				this.gaps.push({ behind: -1, interval: -1, behindLaps: 0, intervalLaps: 0 });
				leadInClass.set(result.car.class, result);
				prevInClass.set(result.car.class, result);
				continue;
			}
			let prev = prevInClass.get(result.car.class);
			if (!prev || !result.carState || !prev.carState) {
				this.gaps.push({ behind: -1, interval: -1, behindLaps: 0, intervalLaps: 0 });
				prevInClass.set(result.car.class, result);
				continue;
			}
			let gap = { behind: -1, interval: -1, behindLaps: 0, intervalLaps: 0 };

			if (result.carState.lapsCompleted == lead.carState.lapsCompleted) {
				gap.behind = result.carState.lapStartET - lead.carState.lapStartET;
			} else {
				gap.behindLaps = lead.carState.lapsCompleted - result.carState.lapsCompleted;
			}

			if (result.carState.lapsCompleted == prev.carState.lapsCompleted) {
				gap.interval = result.carState.lapStartET - prev.carState.lapStartET;
			} else {
				gap.intervalLaps = prev.carState.lapsCompleted - result.carState.lapsCompleted;
			}
			prevInClass.set(result.car.class, result);
			this.gaps.push(gap);
		}
	}

	private calcTimedGaps() {
		this.gaps.length = 0;
		let classTimes = new Map<string, { leader: number, last: number }>();
		for (let result of this.results) {
			let lap = result.bestLap;
			if (!lap) {
				this.gaps.push({ behind: -1, interval: -1, behindLaps: 0, intervalLaps: 0 });
				continue;
			}

			let times = (classTimes.get(result.car.class) ?? { leader: -1, last: -1 });
			if (times.leader < 0 || lap.totalTime <= 0)
				this.gaps.push({ behind: -1, interval: -1, behindLaps: 0, intervalLaps: 0 });
			else
				this.gaps.push({ behind: lap.totalTime - times.leader, interval: lap.totalTime - times.last, behindLaps: 0, intervalLaps: 0 });
			if (times.leader < 0)
				classTimes.set(result.car.class, { leader: lap.totalTime, last: lap.totalTime });
			else
				times.last = lap.totalTime;
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
				this.carStatusDesc = TimingService.getCarDescription(car, { driverName: result.bestLap?.driver } as Standing);
			} else {
				this.pitSummary()?.clear();
				this.carStatusDesc = undefined;
			}
		}
	}

	getBestClasses(key: string, lap: Lap | undefined, carClass: string) {
		let bestClasses: BestClasses = { total: null, sector1: null, sector2: null, sector3: null };
		if (!lap)
			return bestClasses;
		let total = lap.totalTime;
		let sector1 = lap.sector1;
		let sector2 = lap.sector2;
		let sector3 = lap.sector3;
		if (this.session?.bests) {
			let bests = this.session.bests;
			let classBest = bests.class[carClass] ?? this.defaultBest();
			let carBest = bests.car[key] ?? this.defaultBest();
			let driverBest = (bests.driver[key] ? bests.driver[key][lap.driver] : null) ?? this.defaultBest();
			bestClasses.total = total <= 0 ? null : total <= classBest.total ? 'best-class' : null;
			bestClasses.sector1 = sector1 <= 0 ? null : sector1 <= classBest.sector1 ? 'best-class' : sector1 <= carBest.sector1 ? 'best-car' : sector1 <= driverBest.sector1 ? 'best-driver' : null;
			bestClasses.sector2 = sector2 <= 0 ? null : sector2 <= classBest.sector2 ? 'best-class' : sector2 <= carBest.sector2 ? 'best-car' : sector2 <= driverBest.sector2 ? 'best-driver' : null;
			bestClasses.sector3 = sector3 <= 0 ? null : sector3 <= classBest.sector3 ? 'best-class' : sector3 <= carBest.sector3 ? 'best-car' : sector3 <= driverBest.sector3 ? 'best-driver' : null;
		}
		return bestClasses;
	}

	private defaultBest() {
		return { total: -1, sector1: -1, sector2: -1, sector3: -1 } as Best;
	}
}
