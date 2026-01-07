import { Component, inject, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ServerApiService } from '../../server-api.service';

@Component({
	selector: 'app-session-laps',
	imports: [],
	templateUrl: './laps.html',
	styleUrl: './laps.css',
})
export class Laps {
	private ref = inject(ChangeDetectorRef);
	private route = inject(ActivatedRoute);
	private api = inject(ServerApiService);
	carHistory: any = null;

	constructor() {
		let sessionId = this.route.snapshot.paramMap.get('sessionId');
		let carId = this.route.snapshot.paramMap.get('carId');
		if(!sessionId || !carId)
			return;
		this.api.getLaps(sessionId, carId).then(result => {
			this.carHistory = result;
			this.ref.markForCheck();
		}, error => { console.log(error); })
	}
}
