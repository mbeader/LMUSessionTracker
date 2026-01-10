import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { ActivatedRoute, ParamMap, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ServerApiService } from '../server-api.service';
import { Lap } from '../models';
import { Format } from '../format';
import { ClassBadge } from '../session/class-badge/class-badge';

declare var bootstrap: any;
@Component({
	selector: 'app-best-laps',
	imports: [FormsModule, RouterLink, ClassBadge],
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
	laps: Lap[] | null = null;
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
	network: string = '';
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
					this.setFilters(snapshot.queryParamMap);
					this.changeTrack(true, true);
				});
			}
		}, error => { console.log(error); });
	}

	onChange(e: Event) {
		if (e && e.target && e.target instanceof HTMLSelectElement) {
			this.changeTrack(true, false);
		}
	}

	changeTrack(nagivate: boolean, replaceUrl: boolean) {
		this.query(nagivate, replaceUrl);
		if (this.track)
			this.api.getBestLaps(this.track, this.network, this.classes.filter(x => x.checked).map(x => x.value)).then(result => {
				this.laps = result ?? [];
				this.ref.markForCheck();
			}, error => { console.log(error); });
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
		this.network = Array.from(form.querySelectorAll<HTMLInputElement>('input[name="network"]')).find(x => x.checked)?.value ?? '';
		let classes = Array.from(form.querySelectorAll<HTMLInputElement>('#classset input')).map(x => { return { value: x.value, checked: x.checked }; });
		for (let carClass of this.classes) {
			carClass.checked = classes.find(x => x.value == carClass.value)?.checked ?? false;
		}
	}

	setFilters(queryParamMap: ParamMap) {
		this.track = queryParamMap.get('track') ?? this.tracks?.[0] ?? '';

		let form = document.querySelector('#filterModal .modal-body');
		if (!form) {
			return;
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
	}

	query(nagivate: boolean, replaceUrl: boolean) {
		if (nagivate)
			this.router.navigate(['.'], {
				relativeTo: this.route,
				queryParams: {
					track: this.track,
					classes: this.classes.filter(x => x.checked).map(x => x.value),
					network: this.network
				},
				queryParamsHandling: 'replace',
				replaceUrl: replaceUrl
			});
	}
}
