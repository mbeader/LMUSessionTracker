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
	gaps: { behind: number, interval: number }[] = [];
	results: Lap[] = [];
	isRace: boolean = false;
	Format = Format;
	CarKey = CarKey;

	ngOnChanges() {
		this.isRace = this.session?.session?.sessionType.startsWith('RACE') ?? false;
		if (this.session?.session?.sessionId) {
			this.results = this.session.results ?? [];
			this.positionInClass.clear();
			let classCount = new Map<string, number>();
			let classTimes = new Map<string, { leader: number, last: number }>();
			for (let lap of this.results) {
				let key = CarKey.fromCar(lap.car);
				let pic = (classCount.get(lap.car.class) ?? 0) + 1;
				classCount.set(lap.car.class, pic);
				this.positionInClass.set(key.id, pic);

				let times = (classTimes.get(lap.car.class) ?? { leader: -1, last: -1 });
				if(times.leader < 0 || lap.totalTime <= 0)
					this.gaps.push({ behind: -1, interval: -1 });
				else
					this.gaps.push({ behind: lap.totalTime - times.leader, interval: lap.totalTime - times.last });
				if(times.leader < 0)
					classTimes.set(lap.car.class, { leader: lap.totalTime, last: lap.totalTime });
				else
					times.last = lap.totalTime;
			}
			this.ref.markForCheck();
		}
	}
}
