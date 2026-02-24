import { ChangeDetectorRef, Component, inject, Input } from '@angular/core';
import { BestClasses, TimingService } from '../timing.service';
import { SessionViewModel } from '../../view-models';
import { Car, CarKey, CarState, Pit } from '../../tracking';
import { Standing } from '../../lmu';
import { Format } from '../../format';
import { ClassBadge } from '../class-badge/class-badge';

@Component({
	selector: 'app-session-car-status',
	imports: [ClassBadge],
	templateUrl: './car-status.html',
	styleUrl: './car-status.css',
})
export class CarStatus {
	private ref = inject(ChangeDetectorRef);
	private timingService?: TimingService;
	//@Input() lap: LapModel | null = null;
	id?: string;
	session?: SessionViewModel;
	car?: Car;
	state?: CarState;
	standing?: Standing;
	carClass: string = '';
	lastClasses: BestClasses = this.defaultBestClasses();
	bestClasses: BestClasses = this.defaultBestClasses();
	positionInClass: number = 0;
	lastStop?: Pit;
	lastSwap?: Pit;
	Format = Format;

	init(timingService: TimingService) {
		this.timingService = timingService;
	}

	clear() {
		this.id = undefined;
		this.ref.markForCheck();
	}

	setCar(id: string) {
		this.id = id;
		this.onChange();
	}

	onChange() {
		if (!this.timingService)
			return;
		this.session = this.timingService.session ?? undefined;
		this.lastStop = undefined;
		this.lastSwap = undefined;
		if (this.id) {
			this.car = this.timingService.entries.get(this.id);
			this.state = this.timingService.carState.get(this.id);
			if (this.car) {
				this.standing = this.session?.standings?.find(x => CarKey.fromStanding(x).id == this.id);
				this.carClass = (this.car && this.car.class ? this.car.class : this.standing?.carClass) ?? '';
				if (this.standing) {
					this.lastClasses = this.timingService.getLastClasses(this.standing, this.carClass);
					this.bestClasses = this.timingService.getBestClasses(this.standing, this.carClass);
					this.positionInClass = this.timingService.positionInClass.get(this.id) ?? 0;
				} else {
					this.standing = undefined;
					this.lastClasses = this.defaultBestClasses();
					this.bestClasses = this.defaultBestClasses();
				}
				let pits = this.session?.history?.find(x => x.key == this.id)?.pits;
				if (pits) {
					for (let pit of pits) {
						if (pit.stopTime >= 0)
							this.lastStop = pit;
						if (pit.swapTime >= 0)
							this.lastSwap = pit;
					}
				}
			} else {
				this.car = undefined;
				this.state = undefined;
				this.standing = undefined;
				this.carClass = '';
				this.lastClasses = this.defaultBestClasses();
				this.bestClasses = this.defaultBestClasses();
			}
		} else {
			this.car = undefined;
			this.state = undefined;
			this.standing = undefined;
			this.carClass = '';
			this.lastClasses = this.defaultBestClasses();
			this.bestClasses = this.defaultBestClasses();
		}
		this.ref.markForCheck();
	}

	private defaultBestClasses() {
		return { total: null, sector1: null, sector2: null, sector3: null } as BestClasses;
	}
}
