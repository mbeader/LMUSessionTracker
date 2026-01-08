import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { SessionViewModel } from '../../view-models';
import { CarKey } from '../../tracking';

@Component({
	selector: 'app-session-history',
	imports: [RouterLink],
	templateUrl: './history.html',
	styleUrl: './history.css',
})
export class History {
	@Input() session: SessionViewModel | null = null;
	drivers: Map<string, Set<string>> = new Map();
	carKeyId = (key: CarKey | undefined) => key ? `${key.slotId}-${key.veh}` : null;
	join = (set: Set<string> | undefined) => set ? Array.from(set).join(', ') : null;

	ngOnChanges() {
		this.drivers.clear();
		if (this.session && this.session.history) {
			for (let car of this.session.history) {
				let set: Set<string> = new Set();
				let key = this.carKeyId(car.key);
				if (key) {
					for (let lap of car.laps)
						set.add(lap.driver);
					this.drivers.set(key, set);
				}
			}
		}
	}
}
