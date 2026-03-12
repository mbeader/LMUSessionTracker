import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { TimingCarInfo, TimingService } from '../timing.service';
import { Format } from '../../format';
import { statusClass } from '../../utils';
import { ClassBadge } from '../../cars/class-badge/class-badge.component';
import { BrandBadge } from '../../cars/brand-badge/brand-badge.component';

@Component({
	selector: 'app-session-car-status',
	imports: [ClassBadge, BrandBadge],
	templateUrl: './car-status.component.html',
	styleUrl: './car-status.component.css',
})
export class CarStatus {
	private ref = inject(ChangeDetectorRef);
	private timingService?: TimingService;
	id?: string;
	info?: TimingCarInfo;
	Format = Format;
	Utils = { statusClass };

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
		this.info = this.id ? this.timingService.getCar(this.id) : new TimingCarInfo();
		this.ref.markForCheck();
	}
}
