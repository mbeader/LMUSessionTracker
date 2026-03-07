import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Car } from '../../models';
import { ServerApiService, ServerApiServiceToken } from '../../server-api.service/server-api.service';
import { ClassBadge } from '../class-badge/class-badge';
import { getFlag } from '../../utils';

@Component({
	selector: 'app-session-entry-list',
	imports: [ClassBadge],
	templateUrl: './entry-list.html',
	styleUrl: './entry-list.css',
})
export class EntryList {
	private ref = inject(ChangeDetectorRef);
	private route = inject(ActivatedRoute);
	private api = inject(ServerApiServiceToken);
	cars: Car[] | null = null;
	Utils = { getFlag };

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
}
