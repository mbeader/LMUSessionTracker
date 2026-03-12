import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { NgbPopover } from '@ng-bootstrap/ng-bootstrap';
import { ServerApiService, ServerApiServiceToken } from '../../data/server-api/server-api.service';
import { SessionEntry } from '../../view-models';
import { CarKey } from '../../tracking';
import { getBadge, getFlag } from '../../utils';
import { ClassBadge } from '../../cars/class-badge/class-badge.component';
import { BrandBadge } from '../../cars/brand-badge/brand-badge.component';

@Component({
	selector: 'app-session-entry-list',
	imports: [ClassBadge, BrandBadge, NgbPopover, RouterLink],
	templateUrl: './entry-list.component.html',
	styleUrl: './entry-list.component.css',
})
export class EntryList {
	private ref = inject(ChangeDetectorRef);
	private route = inject(ActivatedRoute);
	private api = inject(ServerApiServiceToken);
	entries: SessionEntry[] | null = null;
	Utils = { getFlag, getBadge };
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
