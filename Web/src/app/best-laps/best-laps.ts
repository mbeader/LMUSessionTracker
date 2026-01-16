import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ServerApiService } from '../server-api.service';
import { BestLap, ClassBest, Lap as LapModel } from '../models';
import { Format } from '../format';
import { BestLapsFilters } from '../view-models';
import { ClassBadge } from '../session/class-badge/class-badge';
import { Lap } from './lap/lap';

declare var bootstrap: any;
@Component({
	selector: 'app-best-laps',
	imports: [FormsModule, ClassBadge, Lap],
	templateUrl: './best-laps.html',
	styleUrl: './best-laps.css',
})
export class BestLaps {
	private route = inject(ActivatedRoute);
	private router = inject(Router);
	private ref = inject(ChangeDetectorRef);
	private api = inject(ServerApiService);
	private filterModal: any;
	tracks: string[] | null = null;
	laps: BestLap[] | null = null;
	bests: { [key: string]: ClassBest } | null = null;
	track: string = '';
	classes: { value: string, checked: boolean }[] = [
		{ value: 'Hyper', checked: true },
		{ value: 'LMP2', checked: true },
		{ value: 'LMP2_ELMS', checked: true },
		{ value: 'LMP3', checked: true },
		{ value: 'GTE', checked: true },
		{ value: 'GT3', checked: true },
		{ value: 'Unknown', checked: true }
	];
	sessions: { value: string, checked: boolean }[] = [
		{ value: 'Practice', checked: true },
		{ value: 'Qualifying', checked: true },
		{ value: 'Race', checked: true }
	];
	network: string = '';
	since: string = new Date(Date.now() - 2592000000).toISOString().split('T')[0];
	sinceMax: string = new Date().toISOString().split('T')[0];
	anytime: boolean = true;
	knownDriversOnly: boolean = false;
	init: boolean = true;
	selectedLap: LapModel | null = null;
	selectedLapType: number = 0;
	Format = Format;

	constructor() {
		this.route.queryParamMap.subscribe(queryParamMap => {
			this.setFilters(queryParamMap);
			this.ref.markForCheck();
			this.changeTrack(false, true);
		});
		this.api.getTracks().then(result => {
			this.tracks = result ?? [];
			this.ref.markForCheck();
			if (this.tracks.length > 0) {
				let snapshot = this.route.snapshot;
				new Promise((resolve) => {
					let fn = (fn: any) => {
						let form = document.querySelector('#filterModal .modal-body');
						if (!form)
							setTimeout(fn, 100);
						else
							resolve(true);
					}
					fn(fn);
				}).then(() => {
					this.init = false;
					this.setFilters(snapshot.queryParamMap);
					this.changeTrack(true, true);
					document.querySelector('#lapModal')?.addEventListener('show.bs.modal', this.updateLapModal.bind(this));
				});
			} else
				this.init = false;
		}, error => { console.log(error); });
	}

	onChange(e: Event) {
		if (e && e.target && e.target instanceof HTMLSelectElement) {
			this.changeTrack(true, false);
		}
	}

	changeTrack(nagivate: boolean, replaceUrl: boolean) {
		this.query(nagivate, replaceUrl);
		if (this.track) {
			let filters = new BestLapsFilters();
			filters.track = this.track;
			filters.since = this.anytime ? null : new Date(this.since).toISOString();
			filters.network = this.network;
			filters.classes = this.classes.filter(x => x.checked).map(x => x.value);
			filters.sessions = this.sessions.filter(x => x.checked).map(x => x.value);
			filters.knownDriversOnly = this.knownDriversOnly;
			this.api.getBestLaps(filters).then(result => {
				this.laps = result.laps ?? [];
				this.bests = result.classBests ?? {};
				this.ref.markForCheck();
			}, error => { console.log(error); });
		}
	}

	applyFilters(e: Event) {
		let target = e.target && e.target instanceof HTMLButtonElement ? e.target : null;
		let form = target?.closest('.modal')?.querySelector('.modal-body');
		if (!form)
			return;
		this.readFilters(form);
		this.query(true, false);
		if (!this.filterModal)
			this.filterModal = new bootstrap.Modal(document.querySelector('#filterModal'));
		this.filterModal.hide();
	}

	readFilters(form: Element) {
		this.anytime = form.querySelector<HTMLInputElement>('input#any-date')?.checked ?? false;
		this.since = form.querySelector<HTMLInputElement>('input#since-date')?.value ?? '';
		this.network = Array.from(form.querySelectorAll<HTMLInputElement>('input[name="network"]')).find(x => x.checked)?.value ?? '';
		let classes = Array.from(form.querySelectorAll<HTMLInputElement>('#classset input')).map(x => { return { value: x.value, checked: x.checked }; });
		for (let carClass of this.classes) {
			carClass.checked = classes.find(x => x.value == carClass.value)?.checked ?? false;
		}
		let sessions = Array.from(form.querySelectorAll<HTMLInputElement>('#sessionsset input')).map(x => { return { value: x.value, checked: x.checked }; });
		for (let session of this.sessions) {
			session.checked = sessions.find(x => x.value == session.value)?.checked ?? false;
		}
		this.knownDriversOnly = Array.from(form.querySelectorAll<HTMLInputElement>('input[name="knownDrivers"]')).find(x => x.checked)?.value == 'true';
	}

	setFilters(queryParamMap: ParamMap) {
		this.track = queryParamMap.get('track') ?? this.tracks?.[0] ?? '';

		let form = document.querySelector('#filterModal .modal-body');
		if (!form) {
			return;
		}

		let date = queryParamMap.get('since');
		if (date) {
			this.since = date;
			this.anytime = false;
		} else
			this.anytime = true;
		let anyCheck = form.querySelector<HTMLInputElement>('input#any-date');
		if (anyCheck)
			anyCheck.checked = this.anytime;
		let sinceInput = form.querySelector<HTMLInputElement>('input#since-date');
		if (sinceInput) {
			if (this.anytime)
				sinceInput.setAttribute('disabled', '');
			else
				sinceInput.removeAttribute('disabled');
		}

		this.network = queryParamMap.get('network') ?? '';
		for (let network of form.querySelectorAll<HTMLInputElement>('input[name="network"]')) {
			if (network.value == this.network)
				network.checked = true;
		}

		let classes = queryParamMap.getAll('classes');
		if (classes.length == 0)
			classes = this.classes.map(x => x.value);
		for (let carClass of form.querySelectorAll<HTMLInputElement>('#classset input')) {
			let checked = classes.includes(carClass.value);
			carClass.checked = checked;
			let carClassObj = this.classes.find(x => x.value == carClass.value);
			if (carClassObj)
				carClassObj.checked = checked;
		}

		let sessions = queryParamMap.getAll('sessions');
		if (sessions.length == 0)
			sessions = this.sessions.map(x => x.value);
		for (let session of form.querySelectorAll<HTMLInputElement>('#sessionsset input')) {
			let checked = sessions.includes(session.value);
			session.checked = checked;
			let sessionObj = this.sessions.find(x => x.value == session.value);
			if (sessionObj)
				sessionObj.checked = checked;
		}

		this.knownDriversOnly = queryParamMap.get('knownDriversOnly') == 'true';
		for (let knownDrivers of form.querySelectorAll<HTMLInputElement>('input[name="knownDrivers"]')) {
			if ((knownDrivers.value == 'true') == this.knownDriversOnly)
				knownDrivers.checked = true;
		}
	}

	query(nagivate: boolean, replaceUrl: boolean) {
		if (nagivate)
			this.router.navigate(['.'], {
				relativeTo: this.route,
				queryParams: {
					track: this.track,
					since: this.anytime ? null : this.since,
					classes: this.classes.filter(x => x.checked).map(x => x.value),
					sessions: this.sessions.filter(x => x.checked).map(x => x.value),
					network: this.network,
					knownDriversOnly: this.knownDriversOnly
				},
				queryParamsHandling: 'replace',
				replaceUrl: replaceUrl
			});
	}

	onChangeDate(e: Event) {
		if (e && e.target && e.target instanceof HTMLInputElement) {
			let checked = e.target.checked;
			let sinceInput = e.target.closest('fieldset')?.querySelector('input[type=date]');
			if (sinceInput) {
				if (checked)
					sinceInput.setAttribute('disabled', '');
				else
					sinceInput.removeAttribute('disabled');
			}
		}
	}

	updateLapModal(e: any) {
		if (e && e.relatedTarget && e.relatedTarget instanceof HTMLButtonElement) {
			let button: HTMLButtonElement = e.relatedTarget;
			if (button.getAttribute('data-bs-target') != '#lapModal')
				return;
			let th = button.closest('tr')?.querySelector('th');
			if (th) {
				let i = parseInt(th.textContent) - 1;
				this.selectedLap = this.laps ? this.laps[i]?.lap : null;
				this.selectedLapType = Array.from(button.parentElement?.children ?? []).indexOf(button);
				this.ref.markForCheck();
			}
		}
	}
}
