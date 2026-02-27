import { Component, inject, Input, viewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CarStatusDescription, TimingService } from '../timing.service';
import { SessionViewModel } from '../../view-models';
import { CarKey } from '../../tracking';
import { Standing } from '../../lmu';
import { Format } from '../../format';
import { classId, statusClass, whenExists } from '../../utils';
import { ClassBadge } from '../class-badge/class-badge';
import { CarStatus } from '../car-status/car-status';
import { PitSummary } from '../pit-summary/pit-summary';

@Component({
	selector: 'app-session-standings',
	imports: [RouterLink, ClassBadge, CarStatus, PitSummary],
	providers: [TimingService],
	templateUrl: './standings.html',
	styleUrl: './standings.css',
})
export class Standings {
	private timingService = inject(TimingService);
	private carStatus = viewChild(CarStatus);
	private pitSummary = viewChild(PitSummary);
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
