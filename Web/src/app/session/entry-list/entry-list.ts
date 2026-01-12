import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Entry } from '../../tracking';
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
	entries: Entry[] | null = null;

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
