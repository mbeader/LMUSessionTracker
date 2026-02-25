import { inject, Injectable } from '@angular/core';
import { Best, Bests, Car, CarKey, CarState, Pit } from '../tracking';
import { SessionViewModel } from '../view-models';
import { Standing } from '../lmu';
import { Format } from '../format';
import { classId, statusClass } from '../utils';
import { SettingsService } from '../settings.service';

export type NullabeString = string | null;
export type BestClasses = { total: NullabeString, sector1: NullabeString, sector2: NullabeString, sector3: NullabeString };
export type BestMap = { [key: string]: Best };
export type CarStatusDescription = { carClass: string, number: string, team: string };

@Injectable()
export class TimingService {
	private settings = inject(SettingsService);
	fields: TimingFields = new TimingFields();
	session: SessionViewModel | null = null;
	entries: Map<string, Car> = new Map();
	carState: Map<string, CarState> = new Map();
	positionInClass: Map<string, number> = new Map();
	classBests: Map<string, number> = new Map();
	bests: Bests | null = null;
	isRace: boolean = false;
	speed: string = 'mph';

	constructor() {
		this.speed = this.settings.get().speed;
	}

	onChange() {
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
		if (this.bests && this.session && this.session.standings) {
			for (let standing of this.session.standings) {
				let id = CarKey.fromStanding(standing).id;
				let car = this.entries.get(id ?? '');
				let carClass = (car && car.class ? car.class : standing.carClass) ?? '';
				this.setIfBest(standing, carClass);
			}
		}
		this.carState.clear();
		if (this.session && this.session.carState) {
			for (let car of this.session.carState) {
				this.carState.set(car.key, car);
			}
		}
	}

	private setIfBest(standing: Standing, carClass: string) {
		if (!this.bests || standing.sector == "SECTOR1")
			return;
		let key = CarKey.fromStanding(standing).id;
		if (!this.bests.class[carClass])
			this.bests.class[carClass] = this.defaultBest();
		if (!this.bests.car[key])
			this.bests.car[key] = this.defaultBest();
		if (!this.bests.driver[key])
			this.bests.driver[key] = new Object() as BestMap;
		if (!this.bests.driver[key][standing.driverName])
			this.bests.driver[key][standing.driverName] = this.defaultBest();

		let total = standing.bestLapTime;
		let sector1 = standing.currentSectorTime1;
		let sector2 = standing.currentSectorTime1 > 0 && standing.currentSectorTime2 > 0 ? standing.currentSectorTime2 - standing.currentSectorTime1 : -1;
		let sector3 = standing.lastSectorTime2 > 0 && standing.lastLapTime > 0 ? standing.lastLapTime - standing.lastSectorTime2 : -1;
		this.setBest(this.bests.class[carClass], total, sector1, sector2, sector3);
		this.setBest(this.bests.car[key], total, sector1, sector2, sector3);
		this.setBest(this.bests.driver[key][standing.driverName], total, sector1, sector2, sector3);
	}

	private setBest(best: Best, total: number, sector1: number, sector2: number, sector3: number) {
		if (total > 0 && (total < best.total || best.total <= 0))
			best.total = total;
		if (sector1 > 0 && (sector1 < best.sector1 || best.sector1 <= 0))
			best.sector1 = sector1;
		if (sector2 > 0 && (sector2 < best.sector2 || best.sector2 <= 0))
			best.sector2 = sector2;
		if (sector3 > 0 && (sector3 < best.sector3 || best.sector3 <= 0))
			best.sector3 = sector3;
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
		if (this.bests) {
			let classBest = this.bests.class[carClass] ?? this.defaultBest();
			bestClasses.total = total <= 0 ? null : total <= classBest.total ? 'best-class' : null;
		}
		return bestClasses;
	}

	getCarDescription(id: string) {
		let car = this.entries.get(id);
		if (!car)
			return;
		let standing = this.session?.standings?.find(x => CarKey.fromStanding(x).id == id);
		let driver = standing?.driverName ?? '';
		let carClass = (car && car.class ? car.class : standing?.carClass) ?? '';
		let number = car?.number ? car.number : standing?.carNumber ?? '?';
		let team = car?.teamName ? car.teamName : (!standing?.fullTeamName ? standing?.vehicleName : standing?.fullTeamName) ?? driver;
		return { carClass: carClass, number: number, team: team } as CarStatusDescription;
	}

	getCar(id: string) {
		let info = new TimingCarInfo();
		info.id = id;
		info.set(this);
		return info;
	}

	getCars() {
		let cars = [];
		if (this.session?.standings) {
			for (let standing of this.session.standings) {
				cars.push(this.getCar(CarKey.fromStanding(standing).id));
			}
		}
		return cars;
	}
}

export interface TimingField {
	id: number;
	name: string;
	desc: string;
	value: (service: TimingCarInfo) => string | null | undefined;
	classes?: (service: TimingCarInfo) => string;
	align?: string;
	colType?: string;
}

class TimingFields {
	private static nextId: number = 50;
	fields: TimingField[] = [
		{
			id: 1,
			name: 'Status',
			desc: 'Car status',
			value: i => Format.status(i.standing),
			classes: i => statusClass(Format.status(i.standing)),
			align: 'center'
		},
		{
			id: 2,
			name: 'Pos',
			desc: 'Position',
			value: i => i.standing?.position.toString(),
			classes: i => i.positionInClass == 1 ? `pic-${classId(i.carClass)}` : '',
			align: 'center'
		},
		{
			id: 3,
			name: 'PIC',
			desc: 'Position in class',
			value: i => i.positionInClass.toString(),
			classes: i => `pic-${classId(i.carClass)}`,
			align: 'center'
		},
		{
			id: 4,
			name: 'St',
			desc: 'Start',
			value: i => i.standing?.qualification.toString(),
			align: 'center'
		},
		{
			id: 5,
			name: 'Class',
			desc: 'Car class',
			value: i => i.carClass,
			align: 'center'
		},
		{
			id: 6,
			name: '#',
			desc: 'Car number',
			value: i => i.car?.number ? i.car.number : i.standing?.carNumber,
			align: 'center'
		},
		{
			id: 7,
			name: 'Team',
			desc: 'Team',
			value: i => i.car?.teamName ? i.car.teamName : (!i.standing?.fullTeamName ? i.standing?.vehicleName : i.standing.fullTeamName),
			classes: () => 'text-truncate',
			colType: 'team-col'
		},
		{
			id: 8,
			name: 'Car',
			desc: 'Car',
			value: i => i.car?.vehicleName,
			classes: () => 'text-truncate',
			colType: 'team-col'
		},
		{
			id: 9,
			name: 'Driver',
			desc: 'Driver',
			value: i => i.standing?.driverName,
			classes: () => 'text-truncate',
			colType: 'driver-col'
		},
		{
			id: 10,
			name: 'Laps',
			desc: 'Laps completed',
			value: i => i.standing?.lapsCompleted.toString(),
			align: 'end'
		},
		{
			id: 11,
			name: 'Behind',
			desc: 'Time behind leader',
			value: i => typeof i.standing === 'undefined' || (!i.isRace && i.standing.timeBehindLeader <= 0) ? '-' : Format.diff(i.standing.lapsBehindLeader, i.standing.timeBehindLeader),
			align: 'end',
			colType: 'time-col'
		},
		{
			id: 12,
			name: 'Int',
			desc: 'Interval from ahead',
			value: i => typeof i.standing === 'undefined' || (!i.isRace && i.standing.timeBehindNext <= 0) ? '-' : Format.diff(i.standing.lapsBehindNext, i.standing.timeBehindNext),
			align: 'end',
			colType: 'time-col'
		},
		{
			id: 13,
			name: 'Curr',
			desc: 'Current lap time',
			value: i => Format.lapTime(i.standing?.timeIntoLap),
			align: 'end',
			colType: 'time-col'
		},
		{
			id: 14,
			name: 'Best',
			desc: 'Best lap time',
			value: i => Format.lapTime(i.standing?.bestLapTime),
			classes: i => i.bestClasses.total ?? '',
			align: 'end',
			colType: 'time-col'
		},
		{
			id: 15,
			name: 'Last',
			desc: 'Last lap time',
			value: i => Format.lapTime(i.standing?.lastLapTime),
			classes: i => i.lastClasses.total ?? '',
			align: 'end',
			colType: 'time-col'
		},
		{
			id: 16,
			name: 'S1',
			desc: 'Sector 1 time',
			value: i => typeof i.standing === 'undefined' ? '-' : Format.sectorTime(0.0, i.standing.sector == 'SECTOR1' ? i.standing.lastSectorTime1 : i.standing.currentSectorTime1),
			classes: i => i.lastClasses.sector1 ?? '',
			align: 'end',
			colType: 'time-col'
		},
		{
			id: 17,
			name: 'S2',
			desc: 'Sector 2 time',
			value: i => typeof i.standing === 'undefined' ? '-' : i.standing.sector == 'SECTOR2' ? '' : i.standing.sector == 'SECTOR1' ? Format.sectorTime(i.standing.lastSectorTime1, i.standing.lastSectorTime2) : Format.sectorTime(i.standing.currentSectorTime1, i.standing.currentSectorTime2),
			classes: i => i.lastClasses.sector2 ?? '',
			align: 'end',
			colType: 'time-col'
		},
		{
			id: 18,
			name: 'S3',
			desc: 'Sector 3 time',
			value: i => typeof i.standing === 'undefined' ? '-' : i.standing.sector != 'SECTOR1' ? '' : Format.sectorTime(i.standing.lastSectorTime2, i.standing.lastLapTime),
			classes: i => i.lastClasses.sector3 ?? '',
			align: 'end',
			colType: 'time-col'
		},
		{
			id: 19,
			name: 'Speed',
			desc: 'Current speed',
			value: i => typeof i.standing?.lapStartET === 'undefined' ? '-' : Format.speed(i.standing?.carVelocity.velocity, i.speed),
			align: 'end',
			colType: 'char5-col'
		},
		{
			id: 20,
			name: 'Fuel',
			desc: 'Current fuel',
			value: i => typeof i.standing === 'undefined' ? '-' : Format.percent(i.standing.fuelFraction),
			classes: i => typeof i.standing === 'undefined' || i.standing.fuelFraction > .1 ? '' : i.standing.fuelFraction > .05 ? 'text-warning' : 'text-danger',
			align: 'end',
			colType: 'char7-col'
		},
		{
			id: 21,
			name: 'Pit',
			desc: 'Pit count',
			value: i => i.standing?.pitstops.toString(),
			align: 'end',
			colType: 'char2-col'
		},
		{
			id: 22,
			name: 'Pen',
			desc: 'Penalty count (active)',
			value: i => i.standing?.penalties.toString(),
			align: 'end',
			colType: 'char2-col'
		},
		{
			id: 44,
			name: 'LSP',
			desc: 'Laps since pit',
			value: i => i.standing ? (!i.lastStop ? i.standing.lapsCompleted : i.lastStop.lap > i.standing.lapsCompleted ? 0 : i.standing.lapsCompleted - i.lastStop.lap).toString() : null,
			align: 'end',
			colType: 'char2-col'
		},
		{
			id: 45,
			name: 'SLP',
			desc: 'Since last pit (from pit entry of stop)',
			value: i => Format.time(!i.lastStop ? i.currentET : i.currentET - i.lastStop.pitTime),
			align: 'end',
			colType: 'time-col'
		},
		{
			id: 46,
			name: 'LSS',
			desc: 'Laps since swap',
			value: i => i.standing ? (!i.lastSwap ? i.standing.lapsCompleted : i.lastSwap.lap > i.standing.lapsCompleted ? 0 : i.standing.lapsCompleted - i.lastSwap.lap).toString() : null,
			align: 'end',
			colType: 'char2-col'
		},
		{
			id: 47,
			name: 'SLS',
			desc: 'Since last swap (from pit entry of stop)',
			value: i => Format.time(!i.lastSwap ? i.currentET : i.currentET - i.lastSwap.pitTime),
			align: 'end',
			colType: 'time-col'
		},
		{
			id: 23,
			name: 'LTL',
			desc: 'Last trip through pit lane lap',
			value: i => typeof i.state === 'undefined' || i.state.lastPitLap < 0 ? '-' : i.state.lastPitLap.toString(),
			align: 'end',
			colType: 'char2-col'
		},
		{
			id: 24,
			name: 'LTET',
			desc: 'Last trip through pit lane elasped time',
			value: i => typeof i.state === 'undefined' || i.state.lastPitTime < 0 ? '-' : Format.time(i.state.lastPitTime),
			align: 'end',
			colType: 'time-col'
		},
		{
			id: 25,
			name: 'TTL',
			desc: 'Trip through pit lane this lap',
			value: i => i.state?.pitThisLap ? 'P' : '',
			align: 'end',
			colType: 'char2-col'
		},
		{
			id: 26,
			name: 'LPL',
			desc: 'Last pit stop lap',
			value: i => typeof i.state === 'undefined' || i.state.lastStopLap < 0 ? '-' : i.state.lastStopLap.toString(),
			align: 'end',
			colType: 'char2-col'
		},
		{
			id: 27,
			name: 'LPET',
			desc: 'Last pit stop elasped time',
			value: i => typeof i.state === 'undefined' || i.state.lastStopTime < 0 ? '-' : Format.time(i.state.lastStopTime),
			align: 'end',
			colType: 'time-col'
		},
		{
			id: 28,
			name: 'PTL',
			desc: 'Pit stop this lap',
			value: i => i.state?.stopThisLap ? 'P' : '',
			align: 'end',
			colType: 'char2-col'
		},
		{
			id: 29,
			name: 'LPRET',
			desc: 'Last pit stop release (end) elasped time',
			value: i => typeof i.state === 'undefined' || i.state.lastReleaseTime < 0 ? '-' : Format.time(i.state.lastReleaseTime),
			align: 'end',
			colType: 'time-col'
		},
		{
			id: 30,
			name: 'GTL',
			desc: 'Garage this lap',
			value: i => i.state?.garageThisLap ? 'G' : '',
			align: 'end',
			colType: 'char2-col'
		},
		{
			id: 31,
			name: 'LSL',
			desc: 'Last swap lap',
			value: i => typeof i.state === 'undefined' || i.state.lastSwapLap < 0 ? '-' : i.state.lastSwapLap.toString(),
			align: 'end',
			colType: 'char2-col'
		},
		{
			id: 32,
			name: 'LSET',
			desc: 'Last swap elasped time',
			value: i => typeof i.state === 'undefined' || i.state.lastSwapTime < 0 ? '-' : Format.time(i.state.lastSwapTime),
			align: 'end',
			colType: 'time-col'
		},
		{
			id: 33,
			name: 'STL',
			desc: 'Swap this lap',
			value: i => i.state?.swapThisLap ? 'S' : '',
			align: 'end',
			colType: 'char2-col'
		},
		{
			id: 34,
			name: 'SL',
			desc: 'Swap location',
			value: i => typeof i.state === 'undefined' || i.state.swapLocation < 0 ? '-' : i.state.swapLocation.toString(),
			align: 'end',
			colType: 'char2-col'
		},
		{
			id: 35,
			name: 'SP',
			desc: 'Started lap in pit',
			value: i => i.state?.startedLapInPit ? 'P' : '',
			align: 'end',
			colType: 'char2-col'
		},
		{
			id: 48,
			name: '!TL',
			desc: 'Penalty this lap',
			value: i => i.state?.penaltyThisLap ? '!' : '',
			align: 'end',
			colType: 'char2-col'
		},
		{
			id: 36,
			name: 'TPen',
			desc: 'Total penalty count',
			value: i => i.state?.totalPenalties.toString(),
			align: 'end',
			colType: 'char2-col'
		},
		{
			id: 37,
			name: 'TTrp',
			desc: 'Total trip through pit lane count',
			value: i => i.state?.totalPits.toString(),
			align: 'end',
			colType: 'char2-col'
		},
		{
			id: 38,
			name: 'TPit',
			desc: 'Total pit stop count',
			value: i => i.state?.totalStops.toString(),
			align: 'end',
			colType: 'char2-col'
		},
		{
			id: 39,
			name: 'LLET',
			desc: 'Last lap elasped time',
			value: i => typeof i.standing === 'undefined' || i.standing.lapStartET < 0 ? '-' : Format.time(i.standing.lapStartET),
			align: 'end',
			colType: 'time-col'
		},
		{
			id: 40,
			name: 'Lap%',
			desc: 'Current lap progress (%)',
			value: i => Format.distance(i.standing?.lapDistance ?? 0, i.session?.session?.lapDistance ?? 0),
			classes: i => `lap-prog-${Format.lapProgress(i.standing?.lapDistance ?? 0, i.session?.session?.lapDistance ?? 0)}`,
			align: 'center'
		},
		{
			id: 41,
			name: 'SS',
			desc: 'Server scored',
			value: i => i.standing?.serverScored ? '' : 'N',
			align: 'center'
		},
		{
			id: 42,
			name: 'Y',
			desc: 'Under yellow',
			value: i => i.standing?.underYellow ? 'Y' : '',
			align: 'center'
		},
		{
			id: 43,
			name: 'VEH',
			desc: 'VEH file',
			value: i => i.car?.veh
		},
		{
			id: 49,
			name: 'Slot',
			desc: 'Slot ID',
			value: i => i.car?.slotId.toString()
		}
	];
	private idMap = new Map<number, TimingField>();
	private nameMap = new Map<string, TimingField>();

	constructor() {
		for (let field of this.fields) {
			if (this.idMap.get(field.id))
				throw new Error(`Timing field conflict on ID: ${field.id}`);
			if (this.nameMap.get(field.name))
				throw new Error(`Timing field conflict on name: ${field.name}`);
			if (field.id >= TimingFields.nextId)
				throw new Error(`Timing field ID exceeded expected (<${TimingFields.nextId}): ${field.id}`);
			this.idMap.set(field.id, field);
			this.nameMap.set(field.name, field);
		}
	}

	byId(id: number) {
		return this.idMap.get(id);
	}

	byName(name: string) {
		return this.nameMap.get(name);
	}
}

export class TimingCarInfo {
	id?: string;
	session?: SessionViewModel;
	car?: Car;
	state?: CarState;
	standing?: Standing;
	carClass: string = '';
	lastClasses: BestClasses = this.defaultBestClasses();
	bestClasses: BestClasses = this.defaultBestClasses();
	positionInClass: number = 0;
	lastStop?: Pit;
	lastSwap?: Pit;
	isRace: boolean = false;
	currentET: number = 0;
	speed: string = '';

	private defaultBestClasses() {
		return { total: null, sector1: null, sector2: null, sector3: null } as BestClasses;
	}

	set(timingService: TimingService) {
		this.session = timingService.session ?? undefined;
		this.isRace = timingService.isRace;
		this.currentET = this.session?.info?.currentEventTime ?? this.session?.sessionState?.currentEventTime ?? 0;
		this.speed = timingService.speed;
		this.lastStop = undefined;
		this.lastSwap = undefined;
		if (this.id) {
			this.car = timingService.entries.get(this.id);
			this.state = timingService.carState.get(this.id);
			if (this.car) {
				this.standing = this.session?.standings?.find(x => CarKey.fromStanding(x).id == this.id);
				this.carClass = (this.car && this.car.class ? this.car.class : this.standing?.carClass) ?? '';
				if (this.standing) {
					this.lastClasses = timingService.getLastClasses(this.standing, this.carClass);
					this.bestClasses = timingService.getBestClasses(this.standing, this.carClass);
					this.positionInClass = timingService.positionInClass.get(this.id) ?? 0;
				} else {
					this.standing = undefined;
					this.lastClasses = this.defaultBestClasses();
					this.bestClasses = this.defaultBestClasses();
				}
				let pits = this.session?.history?.find(x => x.key == this.id)?.pits;
				if (pits) {
					for (let pit of pits) {
						if (pit.stopTime >= 0)
							this.lastStop = pit;
						if (pit.swapTime >= 0)
							this.lastSwap = pit;
					}
				}
			} else {
				this.car = undefined;
				this.state = undefined;
				this.standing = undefined;
				this.carClass = '';
				this.lastClasses = this.defaultBestClasses();
				this.bestClasses = this.defaultBestClasses();
			}
		} else {
			this.car = undefined;
			this.state = undefined;
			this.standing = undefined;
			this.carClass = '';
			this.lastClasses = this.defaultBestClasses();
			this.bestClasses = this.defaultBestClasses();
		}
	}
}
