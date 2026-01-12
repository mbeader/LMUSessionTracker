import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Car } from '../../models';
import { ServerApiService } from '../../server-api.service';
import { ClassBadge } from '../class-badge/class-badge';

@Component({
	selector: 'app-session-entry-list',
	imports: [ClassBadge],
	templateUrl: './entry-list.html',
	styleUrl: './entry-list.css',
})
export class EntryList {
	private ref = inject(ChangeDetectorRef);
	private route = inject(ActivatedRoute);
	private api = inject(ServerApiService);
	cars: Car[] | null = null;

	constructor() {
		let snapshot = this.route.snapshot;
		let sessionId = snapshot.paramMap.get('sessionId');
		if (!sessionId)
			return;
		this.api.getEntryList(sessionId).then(result => {
			this.cars = result;
			this.ref.markForCheck();
		}, error => { console.log(error); })
	}

	/**
	 * Converts team member nationality (mostly ISO 3166-1-alpha-2) to available flag icon
	 */
	getFlag(country: string) {
		if(!country)
			return 'none';
		country = country.toLowerCase();
		switch (country) {
			case 'ac':
				return 'sh-ac';
			case 'ea':
				return 'es';
			case 'sh':
				return 'sh-hl';
			case 'ta':
				return 'sh-ta';
		}
		return country;
	}
}
