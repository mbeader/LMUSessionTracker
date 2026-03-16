import { Component, inject, Input, viewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CarStatusDescription, TimingService } from '../timing.service';
import { SessionViewModel } from '../../view-models';
import { CarKey } from '../../tracking';
import { Standing } from '../../lmu';
import { Format } from '../../format';
import { classId, statusClass, whenExists } from '../../utils';
import { ClassBadgeComponent } from '../../cars/class-badge/class-badge.component';
import { BrandBadgeComponent } from '../../cars/brand-badge/brand-badge.component';
import { TireBadgeComponent } from '../../cars/tire-badge.component/tire-badge.component';
import { CarStatusComponent } from '../car-status/car-status.component';
import { PitSummaryComponent } from '../pit-summary/pit-summary.component';

@Component({
	selector: 'app-sessions-standings',
	imports: [RouterLink, ClassBadgeComponent, BrandBadgeComponent, TireBadgeComponent, CarStatusComponent, PitSummaryComponent],
	providers: [TimingService],
	templateUrl: './standings.component.html',
	styleUrl: './standings.component.css',
})
export class StandingsComponent {
	private timingService = inject(TimingService);
	private carStatus = viewChild(CarStatusComponent);
	private pitSummary = viewChild(PitSummaryComponent);
	@Input() session: SessionViewModel | null = null;
	carId?: string;
	carStatusDesc?: CarStatusDescription;
	Format = Format;
	Utils = { classId, statusClass };
	CarKey = CarKey;

	constructor() {
		whenExists('#statusModal', el => el.addEventListener('show.bs.modal', this.updateCarStatusModal.bind(this)));
		whenExists('#pitModal', el => el.addEventListener('show.bs.modal', this.updatePitModal.bind(this)));
	}

	ngOnInit() {
		this.carStatus()?.init(this.timingService);
	}

	ngOnChanges() {
		this.timingService.session = this.session;
		this.timingService.onChange();
		this.carStatus()?.onChange();
		this.pitSummary()?.onChange();
	}

	ngOnDestroy() {
		document.querySelector('.modal-backdrop')?.remove();
	}

	get entries() { return this.timingService.entries; }
	get positionInClass() { return this.timingService.positionInClass; }
	get classBests() { return this.timingService.classBests; }
	get bests() { return this.timingService.bests; }
	get isRace() { return this.timingService.isRace; }

	getLastClasses(standing: Standing, carClass: string) { return this.timingService.getLastClasses(standing, carClass); }
	getBestClasses(standing: Standing, carClass: string) { return this.timingService.getBestClasses(standing, carClass); }
	getCar(id: string) { return this.timingService.getCar(id); }

	updateCarStatusModal(e: any) {
		if (e && e.relatedTarget && e.relatedTarget instanceof HTMLButtonElement) {
			let button: HTMLButtonElement = e.relatedTarget;
			if (button.getAttribute('data-bs-target') != '#statusModal')
				return;
			let id = button.closest('tr')?.getAttribute('car-id');
			if (id) {
				this.carId = id;
				this.carStatus()?.setCar(id);
				this.carStatusDesc = this.timingService.getCarDescription(id);
			} else {
				this.carStatus()?.clear();
				this.carStatusDesc = undefined;
			}
		}
	}

	updatePitModal(e: any) {
		if (e && e.relatedTarget && e.relatedTarget instanceof HTMLButtonElement) {
			let button: HTMLButtonElement = e.relatedTarget;
			if (button.getAttribute('data-bs-target') != '#pitModal')
				return;
			let id = button.closest('tr')?.getAttribute('car-id');
			if (id) {
				this.carId = id;
				this.pitSummary()?.setCar(id);
				this.carStatusDesc = this.timingService.getCarDescription(id);
			} else {
				this.pitSummary()?.clear();
				this.carStatusDesc = undefined;
			}
		}
	}
}
