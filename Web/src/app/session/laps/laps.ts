import { Component, inject, ChangeDetectorRef, viewChild } from '@angular/core';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { ServerApiService } from '../../server-api.service';
import { ServerLiveService } from '../../server-live.service';
import { BestClasses } from '../timing.service';
import { LapsViewModel } from '../../view-models';
import { Best, Lap } from '../../tracking';
import { Format } from '../../format';
import { ClassBadge } from '../class-badge/class-badge';
import { PitSummary } from '../pit-summary/pit-summary';

@Component({
	selector: 'app-session-laps',
	imports: [RouterLink, ClassBadge, PitSummary],
	templateUrl: './laps.html',
	styleUrl: './laps.css',
})
export class Laps {
	private ref = inject(ChangeDetectorRef);
	private route = inject(ActivatedRoute);
	private api = inject(ServerApiService);
	private live = inject(ServerLiveService);
	private pitSummary = viewChild(PitSummary);
	model: LapsViewModel | null = null;
	defaultLap = (number: number) => { return { lapNumber: number, totalTime: -1, sector1: -1, sector2: -1, sector3: -1, isValid: false } as Lap };
	Format = Format;

	constructor() {
		let sessionId = this.route.snapshot.paramMap.get('sessionId');
		let carId = this.route.snapshot.paramMap.get('carId');
		if (!sessionId || !carId)
			return;
		this.api.getLaps(sessionId, carId).then(result => {
			this.model = result;
			this.pitSummary()?.setLaps(this.model);
			this.ref.markForCheck();
			if (this.route.snapshot.url[2].path != 'History' && this.model.session?.sessionId && this.model.car?.key)
				this.live.joinLaps(this.model.session.sessionId, this.model.car.key, this.updateLaps.bind(this));
		}, error => { console.log(error); })
	}

	ngOnDestroy() {
		document.querySelector('.modal-backdrop')?.remove();
	}

	updateLaps(laps: LapsViewModel) {
		if (this.model) {
			LapsViewModel.merge(this.model, laps);
			this.pitSummary()?.setLaps(this.model);
			this.ref.markForCheck();
		}
	}

	getBestClasses(lap: Lap) {
		let bestClasses: BestClasses = { total: null, sector1: null, sector2: null, sector3: null };
		let bests = this.model?.bests ?? this.model?.session?.bests;
		if (!bests || !this.model?.car)
			return bestClasses;
		let key = this.model.car.key;
		let carClass = this.model.car.car.class;
		let classBest = bests.class[carClass] ?? this.defaultBest();
		let carBest = bests.car[key] ?? this.defaultBest();
		let driverBest = (bests.driver[key] ? bests.driver[key][lap.driver] : null) ?? this.defaultBest();
		bestClasses.total = this.getBestClass(lap.totalTime, classBest.total, carBest.total, driverBest.total);
		bestClasses.sector1 = this.getBestClass(lap.sector1, classBest.sector1, carBest.sector1, driverBest.sector1);
		bestClasses.sector2 = this.getBestClass(lap.sector2, classBest.sector2, carBest.sector2, driverBest.sector2);
		bestClasses.sector3 = this.getBestClass(lap.sector3, classBest.sector3, carBest.sector3, driverBest.sector3);
		return bestClasses;
	}

	getBestClass(time: number, classTime: number, carTime: number, driverTime: number) {
		return time <= 0 ? null : time <= classTime ? 'best-class' : time <= carTime ? 'best-car' : time <= driverTime ? 'best-driver' : null;
	}

	private defaultBest() {
		return { total: -1, sector1: -1, sector2: -1, sector3: -1 } as Best;
	}
}
