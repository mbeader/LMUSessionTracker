import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { SessionViewModel } from '../../view-models';
import { Car, CarKey } from '../../tracking';
import { Standing } from '../../lmu';
import { Format } from '../../format';

@Component({
	selector: 'app-session-standings',
	imports: [RouterLink],
	templateUrl: './standings.html',
	styleUrl: './standings.css',
})
export class Standings {
	@Input() session: SessionViewModel | null = null;
	entries: Map<string, Car> = new Map();
	positionInClass: Map<string, number> = new Map();
	carKeyId = (key: CarKey | undefined) => key ? `${key.slotId}-${key.veh}` : null;
	carKey = (standing: Standing) => { return { slotId: standing.slotID, veh: standing.vehicleFilename } as CarKey; };
	Format = Format;

	ngOnChanges() {
		this.entries.clear();
		if (this.session && this.session.entries) {
			for (let car of this.session.entries) {
				this.entries.set(this.carKeyId(car[0]) ?? '', car[1]);
			}
		}
		this.positionInClass.clear();
		if (this.session && this.session.positionInClass) {
			for (let car of this.session.positionInClass) {
				this.positionInClass.set(this.carKeyId(car[0]) ?? '', car[1]);
			}
		}
	}
}
