import { ChangeDetectorRef, Component, inject, Input } from '@angular/core';
import { NgbPopover } from '@ng-bootstrap/ng-bootstrap';
import { LapsViewModel, SessionViewModel } from '../../view-models';
import { CarHistory, Pit } from '../../tracking';
import { Format } from '../../format';

@Component({
	selector: 'app-session-pit-summary',
	imports: [NgbPopover],
	templateUrl: './pit-summary.html',
	styleUrl: './pit-summary.css',
})
export class PitSummary {
	private ref = inject(ChangeDetectorRef);
	@Input() laps: LapsViewModel | null = null;
	@Input() session: SessionViewModel | null = null;
	@Input() id?: string;
	car?: CarHistory;
	pitData: PitData[] = [];
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
		this.setPitData();
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

	getTotalPits() {
		let pits = 0;
		for (let pitData of this.pitData) {
			if (!pitData.isGarage)
				pits++;
		}
		return pits;
	}

	getTotalStops() {
		let stops = 0;
		for (let pitData of this.pitData) {
			if (pitData.isStop)
				stops++;
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

	getTotalGarages() {
		let garages = 0;
		for (let pitData of this.pitData) {
			if (pitData.isGarage)
				garages++;
		}
		return garages;
	}

	getTotalTimeStopped() {
		let stopTime = 0;
		for (let pitData of this.pitData) {
			if (pitData.isStop && pitData.stopTime)
				stopTime += pitData.stopTime;
		}
		return stopTime;
	}

	getTotalTimeInPit() {
		let stopTime = 0;
		for (let pitData of this.pitData) {
			if (!pitData.isGarage && pitData.totalTime)
				stopTime += pitData.totalTime;
		}
		return stopTime;
	}

	getTotalTimeInGarage() {
		let stopTime = 0;
		for (let pitData of this.pitData) {
			if (pitData.isGarage && pitData.totalTime)
				stopTime += pitData.totalTime;
		}
		return stopTime;
	}

	getType(pit: Pit) {
		return pit.garageInTime >= 0 || pit.garageOutTime >= 0 ? 'Garage' : pit.stopTime >= 0 ? 'Stop' : null;
	}

	getStopTime(pit: Pit) {
		return pit.time > 0 ? pit.time : pit.stopTime > 0 && pit.releaseTime > 0 ? pit.releaseTime - pit.stopTime : null;
	}

	getTotalTime(pit: Pit, last: boolean) {
		let currentET = (this.id ? this.session?.info?.currentEventTime : this.laps?.currentET ?? this.laps?.session?.currentET ?? (this.laps?.session as any)?.lastInfo?.currentEventTime) ?? -1;
		if (pit.garageInTime >= 0 || pit.garageOutTime >= 0) {
			let out = pit.garageOutTime >= 0 ? pit.garageOutTime : currentET;
			if (pit.garageInTime >= 0 && (pit.garageOutTime >= 0 || (last && currentET >= 0)))
				return out - pit.garageInTime;
			return null;
		} else if (pit.pitTime >= 0) {
			if (pit.exitTime >= 0)
				return pit.exitTime - pit.pitTime;
			if (last && currentET >= 0)
				return currentET - pit.pitTime;
			return null;
		} else
			return null;
	}

	setPitData() {
		this.pitData = [];
		if (!this.car)
			return;
		for (let i = 0; i < this.car.pits.length; i++) {
			let pit = this.car.pits[i];
			let data = { isGarage: false, isStop: false, stopTime: null, totalTime: null } as PitData;
			data.isGarage = pit.garageInTime >= 0 || pit.garageOutTime >= 0;
			data.isStop = !data.isGarage && pit.stopTime >= 0;
			data.stopTime = this.getStopTime(pit);
			data.totalTime = this.getTotalTime(pit, i == this.car.pits.length - 1);
			this.pitData.push(data);
		}
	}
}

interface PitData {
	isGarage: boolean;
	isStop: boolean;
	stopTime: number | null;
	totalTime: number | null;
}
