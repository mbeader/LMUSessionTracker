import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { SessionViewModel } from '../../view-models';
import { Best, Bests, Car, CarKey } from '../../tracking';
import { Format } from '../../format';
import { classId } from '../../utils';
import { ClassBadge } from '../class-badge/class-badge';
import { Standing } from '../../lmu';

type NullabeString = string | null;
type BestClasses = { total: NullabeString, sector1: NullabeString, sector2: NullabeString, sector3: NullabeString };

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
	bests: Bests | null = null;
	isRace: boolean = false;
	Format = Format;
	Utils = { classId };
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
		if (this.session?.bests)
			this.bests = this.session.bests;
	}

	getLastClasses(standing: Standing, carClass: string) {
		let bestClasses: BestClasses = { total: null, sector1: null, sector2: null, sector3: null };
		let s1time = standing.sector == "SECTOR1" ? standing.lastSectorTime1 : standing.currentSectorTime1;
		let s2time = standing.sector == "SECTOR1" ? standing.lastSectorTime2 : standing.currentSectorTime2;
		let total = standing.lastLapTime;
		let sector1 = s1time;
		let sector2 = s1time > 0 && s2time > 0 ? s2time - s1time : -1;
		let sector3 = standing.lastSectorTime2 > 0 && standing.lastLapTime > 0 ? standing.lastLapTime - standing.lastSectorTime2 : -1;
		let key = CarKey.fromStanding(standing).id;
		if (!this.bests) {
			bestClasses.total = total > 0 ? 'best-car' : null;
			bestClasses.sector1 = sector1 > 0 ? 'best-car' : null;
			bestClasses.sector2 = sector2 > 0 ? 'best-car' : null;
			bestClasses.sector3 = sector3 > 0 ? 'best-car' : null;
		} else {
			let classBest = this.bests.class[carClass] ?? this.defaultBest();
			let carBest = this.bests.car[key] ?? this.defaultBest();
			let driverBest = (this.bests.driver[key] ? this.bests.driver[key][standing.driverName] : null) ?? this.defaultBest();
			bestClasses.total = total <= 0 ? null : total <= classBest.total ? 'best-class' : total <= carBest.total ? 'best-car' : total <= driverBest.total ? 'best-driver' : null;
			bestClasses.sector1 = sector1 <= 0 ? null : sector1 <= classBest.sector1 ? 'best-class' : sector1 <= carBest.sector1 ? 'best-car' : sector1 <= driverBest.sector1 ? 'best-driver' : null;
			bestClasses.sector2 = sector2 <= 0 ? null : sector2 <= classBest.sector2 ? 'best-class' : sector2 <= carBest.sector2 ? 'best-car' : sector2 <= driverBest.sector2 ? 'best-driver' : null;
			bestClasses.sector3 = sector3 <= 0 ? null : sector3 <= classBest.sector3 ? 'best-class' : sector3 <= carBest.sector3 ? 'best-car' : sector3 <= driverBest.sector3 ? 'best-driver' : null;
		}
		return bestClasses;
	}

	private defaultBest() {
		return { total: -1, sector1: -1, sector2: -1, sector3: -1 } as Best;
	}

	getBestClasses(standing: Standing, carClass: string) {
		let bestClasses: BestClasses = { total: null, sector1: null, sector2: null, sector3: null };
		let total = standing.bestLapTime;
		if(this.bests) {
			let classBest = this.bests.class[carClass] ?? this.defaultBest();
			bestClasses.total = total <= 0 ? null : total <= classBest.total ? 'best-class' : null;
		}
		return bestClasses;
	}
}
