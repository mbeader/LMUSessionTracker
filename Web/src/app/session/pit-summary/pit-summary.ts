import { ChangeDetectorRef, Component, inject, Input } from '@angular/core';
import { LapsViewModel, SessionViewModel } from '../../view-models';
import { CarHistory } from '../../tracking';
import { Format } from '../../format';

@Component({
	selector: 'app-session-pit-summary',
	imports: [],
	templateUrl: './pit-summary.html',
	styleUrl: './pit-summary.css',
})
export class PitSummary {
	private ref = inject(ChangeDetectorRef);
	@Input() laps: LapsViewModel | null = null;
	@Input() session: SessionViewModel | null = null;
	@Input() id?: string;
	car?: CarHistory;
	Format = Format;

	ngOnInit() {
		this.onChange();
	}

	clear() {
		this.id = undefined;
		this.ref.markForCheck();
	}

	setCar(id: string) {
		this.id = id;
		this.onChange();
		this.ref.markForCheck();
	}

	onChange() {
		if (this.id) {
			this.car = this.session?.history?.find(x => x.key == this.id) ?? undefined;
		} else
			this.car = this.laps?.car ?? undefined;
		this.ref.markForCheck();
	}

	setLaps(laps: LapsViewModel) {
		this.laps = laps;
		this.onChange();
	}

	getPitNumber(index: number) {
		if (this.car?.pits && index >= 0 && index < this.car.pits.length) {
			let stops = 0;
			for (let i = 0; i < this.car.pits.length; i++) {
				let stopped = this.car.pits[i].stopTime >= 0;
				if (stopped)
					stops++;
				if (i == index)
					return stopped ? stops : -1;
			}
		}
		return -1;
	}

	getTotalStops() {
		let stops = 0;
		if (this.car?.pits) {
			for (let i = 0; i < this.car.pits.length; i++) {
				let stopped = this.car.pits[i].stopTime >= 0;
				if (stopped)
					stops++;
			}
		}
		return stops;
	}

	getTotalSwaps() {
		let swaps = 0;
		if (this.car?.pits) {
			for (let i = 0; i < this.car.pits.length; i++) {
				let swapped = this.car.pits[i].swapTime >= 0;
				if (swapped)
					swaps++;
			}
		}
		return swaps;
	}

	getTotalTimeStopped() {
		let stopTime = 0;
		if (this.car?.pits) {
			for (let i = 0; i < this.car.pits.length; i++) {
				if (this.car.pits[i].stopTime > 0 && this.car.pits[i].releaseTime > 0)
					stopTime += this.car.pits[i].releaseTime - this.car.pits[i].stopTime;
			}
		}
		return stopTime;
	}
}
