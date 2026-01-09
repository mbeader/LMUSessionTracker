import { ChangeDetectorRef, Component, inject, Input } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ServerApiService } from '../../server-api.service';
import { SessionViewModel } from '../../view-models';

@Component({
	selector: 'app-session-history',
	imports: [RouterLink],
	templateUrl: './history.html',
	styleUrl: './history.css',
})
export class History {
	private ref = inject(ChangeDetectorRef);
	private route = inject(ActivatedRoute);
	private api = inject(ServerApiService);
	@Input() session: SessionViewModel | null = null;
	drivers: Map<string, Set<string>> = new Map();
	join = (set: Set<string> | undefined) => set ? Array.from(set).join(', ') : null;

	constructor() {
		let snapshot = this.route.snapshot;
		let sessionId = snapshot.paramMap.get('sessionId');
		if (!sessionId)
			return;
		this.api.getSession(sessionId).then(result => {
			this.session = result;
			this.ngOnChanges();
			this.ref.markForCheck();
		}, error => { console.log(error); })
	}

	ngOnChanges() {
		this.drivers.clear();
		if (this.session && this.session.history) {
			for (let car of this.session.history) {
				let set: Set<string> = new Set();
				if (car.key) {
					for (let lap of car.laps)
						if (lap)
							set.add(lap.driver);
					this.drivers.set(car.key, set);
				}
			}
		}
	}
}
