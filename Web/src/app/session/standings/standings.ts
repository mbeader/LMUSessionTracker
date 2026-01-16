import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { SessionViewModel } from '../../view-models';
import { Car, CarKey } from '../../tracking';
import { Format } from '../../format';
import { ClassBadge } from '../class-badge/class-badge';

@Component({
	selector: 'app-session-standings',
	imports: [RouterLink, ClassBadge],
	templateUrl: './standings.html',
	styleUrl: './standings.css',
})
export class Standings {
	@Input() session: SessionViewModel | null = null;
	entries: Map<string, Car> = new Map();
	positionInClass: Map<string, number> = new Map();
	classBests: Map<string, number> = new Map();
	isRace: boolean = false;
	Format = Format;
	CarKey = CarKey;

	ngOnChanges() {
		this.isRace = this.session?.session?.sessionType.startsWith('RACE') ?? false;
		this.entries.clear();
		if (this.session && this.session.entries) {
			for (let car in this.session.entries) {
				this.entries.set(car ?? '', this.session.entries[car]);
			}
		}
		this.positionInClass.clear();
		if (this.session && this.session.positionInClass) {
			for (let car in this.session.positionInClass) {
				this.positionInClass.set(car ?? '', this.session.positionInClass[car]);
			}
		}
		this.classBests.clear();
		if (this.session && this.session.standings && !this.isRace) {
			let lastInClass = new Map<string, number>();
			for (let standing of this.session.standings) {
				let id = CarKey.fromStanding(standing).id;
				let car = this.entries.get(id ?? '');
				let carClass = (car && car.class ? car.class : standing.carClass) ?? '';
				if (this.classBests.has(carClass)) {
					let best = this.classBests.get(carClass) ?? -1;
					let last = lastInClass.get(carClass) ?? -1;
					standing.lapsBehindLeader = 0;
					standing.timeBehindLeader = standing.bestLapTime > 0 && best > 0 ? standing.bestLapTime - best : -1;
					standing.lapsBehindNext = 0;
					standing.timeBehindNext = standing.bestLapTime > 0 && last > 0 ? standing.bestLapTime - last : -1;
					lastInClass.set(carClass, standing.bestLapTime);
				} else {
					this.classBests.set(carClass, standing.bestLapTime);
					standing.lapsBehindLeader = 0;
					standing.timeBehindLeader = 0;
					standing.lapsBehindNext = 0;
					standing.timeBehindNext = 0;
					lastInClass.set(carClass, standing.bestLapTime);
				}
			}
		}
	}
}
