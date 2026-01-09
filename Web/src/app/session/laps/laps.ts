import { Component, inject, ChangeDetectorRef } from '@angular/core';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { ServerApiService } from '../../server-api.service';
import { LapsViewModel } from '../../view-models';
import { Lap } from '../../tracking';
import { Format } from '../../format';

@Component({
	selector: 'app-session-laps',
	imports: [RouterLink],
	templateUrl: './laps.html',
	styleUrl: './laps.css',
})
export class Laps {
	private ref = inject(ChangeDetectorRef);
	private route = inject(ActivatedRoute);
	private api = inject(ServerApiService);
	model: LapsViewModel | null = null;
	defaultLap = (number: number) => { return { lapNumber: number, totalTime: -1, sector1: -1, sector2: -1, sector3: -1, isValid: false } as Lap };
	Format = Format;

	constructor() {
		let sessionId = this.route.snapshot.paramMap.get('sessionId');
		let carId = this.route.snapshot.paramMap.get('carId');
		if(!sessionId || !carId)
			return;
		this.api.getLaps(sessionId, carId).then(result => {
			this.model = result;
			this.ref.markForCheck();
		}, error => { console.log(error); })
	}
}
