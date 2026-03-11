import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { NgbPopover } from '@ng-bootstrap/ng-bootstrap';
import { ServerApiService, ServerApiServiceToken } from '../../server-api.service/server-api.service';
import { SessionEntry } from '../../view-models';
import { CarKey } from '../../tracking';
import { getFlag } from '../../utils';
import { ClassBadge } from '../class-badge/class-badge';
import { BrandBadge } from '../brand-badge/brand-badge';

@Component({
	selector: 'app-session-entry-list',
	imports: [ClassBadge, BrandBadge, NgbPopover, RouterLink],
	templateUrl: './entry-list.html',
	styleUrl: './entry-list.css',
})
export class EntryList {
	private ref = inject(ChangeDetectorRef);
	private route = inject(ActivatedRoute);
	private api = inject(ServerApiServiceToken);
	entries: SessionEntry[] | null = null;
	Utils = { getFlag };
	CarKey = CarKey;

	constructor() {
		let snapshot = this.route.snapshot;
		let sessionId = snapshot.paramMap.get('sessionId');
		if (!sessionId)
			return;
		this.api.getEntryList(sessionId).then(result => {
			this.entries = result;
			this.ref.markForCheck();
		}, error => { console.log(error); })
	}
}
