import { ChangeDetectorRef, Component, inject, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { ServerApiService } from '../../server-api.service';
import { SessionViewModel } from '../../view-models';
import { CarKey } from '../../tracking';
import { Format } from '../../format';
import { Lap } from '../../models';
import { ClassBadge } from '../class-badge/class-badge';

@Component({
	selector: 'app-session-results',
	imports: [RouterLink, ClassBadge],
	templateUrl: './results.html',
	styleUrl: './results.css',
})
export class Results {
	private ref = inject(ChangeDetectorRef);
	private api = inject(ServerApiService);
	@Input() session: SessionViewModel | null = null;
	positionInClass: Map<string, number> = new Map();
	results: Lap[] = [];
	isRace: boolean = false;
	Format = Format;
	CarKey = CarKey;

	ngOnChanges() {
		this.isRace = this.session?.session?.sessionType.startsWith('RACE') ?? false;
		if (this.session?.session?.sessionId)
			this.api.getResults(this.session?.session?.sessionId).then(result => {
				this.results = result;
				this.positionInClass.clear();
				let classCount = new Map<string, number>();
				for (let lap of this.results) {
					let key = CarKey.fromCar(lap.car);
					let pic = (classCount.get(lap.car.class) ?? 0) + 1;
					classCount.set(lap.car.class, pic);
					this.positionInClass.set(key.id, pic);
				}
				this.ref.markForCheck();
			}, error => { console.log(error); })
	}
}
