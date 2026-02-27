import { Component, inject, viewChild } from '@angular/core';
import { RouterLink } from '@angular/router';
import { SessionService } from '../session.service';
import { CarStatusDescription, TimingField, TimingService } from '../timing.service';
import { Format } from '../../format';
import { SessionViewModel } from '../../view-models';
import { Standing } from '../../lmu';
import { CarKey } from '../../tracking';
import { classId, statusClass, whenExists } from '../../utils';
import { ClassBadge } from '../class-badge/class-badge';
import { CarStatus } from '../car-status/car-status';

@Component({
	selector: 'app-session-timing',
	imports: [RouterLink, ClassBadge, CarStatus],
	providers: [SessionService, TimingService],
	templateUrl: './timing.html',
	styleUrl: './timing.css',
})
export class Timing {
	private defaultColumns = [
		1, 2, 3, 5, 6, 7, 9, 10, 11, 12,
		13, 14, 15, 16, 17, 18, 19, 20, 22, 44,
		45, 46, 47, 36, 38, 40
	];
	private service = inject(SessionService);
	private timingService = inject(TimingService);
	private carStatus = viewChild(CarStatus);
	columns: TimingField[] = [];
	carStatusDesc?: CarStatusDescription;
	Utils = { classId, statusClass };
	CarKey = CarKey;
	Format = Format;
	flagClass = SessionViewModel.flagClass;

	get session() { return this.service.session; }
	get hasStandings() { return this.service.hasStandings; }
	get fahrenheit() { return this.service.fahrenheit; }

	constructor() {
		this.service.init(this.onInitSession.bind(this), sessionId => ['/', 'Session', sessionId, 'Timing']);
		whenExists('main', main => { if (main.parentElement) main.parentElement.className = 'container-fluid'; });
		whenExists('#statusModal', el => el.addEventListener('show.bs.modal', this.updateCarStatusModal.bind(this)));
		whenExists('#columnsModal', el => el.addEventListener('show.bs.modal', this.setColumnCheckboxes.bind(this)));
	}

	ngOnInit() {
		this.carStatus()?.init(this.timingService);
		this.columns = this.getColumns(this.readColumns());
	}

	ngOnDestroy() {
		document.querySelector('.modal-backdrop')?.remove();
	}

	private onInitSession(sessionId: string) {
		this.service.getSession(sessionId, this.onUpdateSession.bind(this));
	}

	private onUpdateSession(session: SessionViewModel) {
		this.timingService.session = this.session;
		this.timingService.onChange();
		this.carStatus()?.onChange();
	}

	get entries() { return this.timingService.entries; }
	get carState() { return this.timingService.carState; }
	get positionInClass() { return this.timingService.positionInClass; }
	get classBests() { return this.timingService.classBests; }
	get bests() { return this.timingService.bests; }
	get isRace() { return this.timingService.isRace; }
	get speed() { return this.timingService.speed; }

	getLastClasses(standing: Standing, carClass: string) { return this.timingService.getLastClasses(standing, carClass); }
	getBestClasses(standing: Standing, carClass: string) { return this.timingService.getBestClasses(standing, carClass); }
	getCars() { return this.timingService.getCars(); }

	updateCarStatusModal(e: any) {
		if (e && e.relatedTarget && e.relatedTarget instanceof HTMLButtonElement) {
			let button: HTMLButtonElement = e.relatedTarget;
			if (button.getAttribute('data-bs-target') != '#statusModal')
				return;
			let id = button.closest('tr')?.getAttribute('car-id');
			if (id) {
				this.carStatus()?.setCar(id);
				this.carStatusDesc = this.timingService.getCarDescription(id);
			} else {
				this.carStatus()?.clear();
				this.carStatusDesc = undefined;
			}
		}
	}

	private readColumns() {
		let columns;
		try {
			let json = localStorage.getItem('timing-cols');
			if (json)
				columns = JSON.parse(json);
		} catch {

		}
		if (!columns)
			columns = this.defaultColumns;
		return columns;
	}

	private getColumns(columns: number[]) {
		let cols = [];
		for (let id of columns) {
			let col = this.timingService.fields.byId(id);
			if (col)
				cols.push(col);
		}
		return cols;
	}

	getAllColumns() {
		return this.timingService.fields.fields;
	}

	columnEnabled(id: number) {
		return this.columns.some(x => x.id == id);
	}

	setColumnCheckboxes(e: any) {
		if (e && e.relatedTarget && e.relatedTarget instanceof HTMLButtonElement) {
			let button: HTMLButtonElement = e.relatedTarget;
			if (button.getAttribute('data-bs-target') != '#columnsModal')
				return;
			this.applyColumns();
		}
	}

	private applyColumns() {
		let checks = document.querySelectorAll<HTMLInputElement>('#columnsModal .modal-body input');
		for (let check of checks) {
			let id = parseInt(check.getAttribute('col-id') ?? '');
			check.checked = this.columns.some(x => x.id == id);
		}
	}

	setColumns() {
		let cols = [];
		let checks = document.querySelectorAll<HTMLInputElement>('#columnsModal .modal-body input');
		for (let check of checks) {
			let id = parseInt(check.getAttribute('col-id') ?? '');
			if (check.checked)
				cols.push(id);
		}
		localStorage.setItem('timing-cols', JSON.stringify(cols));
		this.columns = this.getColumns(cols);
	}

	isDefaultColumn(id: number) {
		return this.defaultColumns.includes(id);
	}

	resetColumns() {
		this.columns = this.getColumns(this.defaultColumns);
		this.applyColumns();
		this.setColumns();
	}
}
