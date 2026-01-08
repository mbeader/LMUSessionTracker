import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { SessionViewModel } from '../../view-models';

@Component({
	selector: 'app-session-history',
	imports: [RouterLink],
	templateUrl: './history.html',
	styleUrl: './history.css',
})
export class History {
	@Input() session: SessionViewModel | null = null;
	drivers: Map<string, Set<string>> = new Map();
	join = (set: Set<string> | undefined) => set ? Array.from(set).join(', ') : null;

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
