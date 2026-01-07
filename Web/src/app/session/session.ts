import { Component, inject, ChangeDetectorRef } from '@angular/core';
import { RouterLink, ActivatedRoute } from '@angular/router';
import { ServerApiService } from '../server-api.service';
import { Format } from '../format';

@Component({
	selector: 'app-session',
	imports: [RouterLink],
	templateUrl: './session.html',
	styleUrl: './session.css',
})
export class Session {
	private ref = inject(ChangeDetectorRef);
	private route = inject(ActivatedRoute);
	private api = inject(ServerApiService);
	session: any = null;
	Format = Format;

	constructor() {
		let sessionId = this.route.snapshot.paramMap.get('sessionId');
		if(!sessionId)
			return;
		this.api.getSession(sessionId).then(result => {
			this.session = result;
			this.ref.markForCheck();
		}, error => { console.log(error); })
	}
}
