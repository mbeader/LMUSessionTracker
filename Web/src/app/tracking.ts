import { Standing } from "./lmu";
import { Car as CarModel } from "./models";

export interface Best {
	total: number;
	sector1: number;
	sector2: number;
	sector3: number;
}

export interface Bests {
	class: { [key: string]: Best };
	car: { [key: string]: Best };
	driver: { [key: string]: { [key: string]: Best } };
}

export interface CarHistory {
	/** CarKey */
	key: string;
	car: Car;
	laps: Array<Lap | null>;
	lapsCompleted: number
	pits: Array<Pit>;
}

//export interface CarKey {
//	slotId: number;
//	veh: string;
//}
export class CarKey {
	public slotId: number;
	public veh: string;

	constructor(slotId: number, veh: string) {
		this.slotId = slotId;
		this.veh = veh;
	}

	matches(s: string) {
		return s == this.id;
	}

	get id() {
		return `${this.slotId}-${this.veh}`;
	}

	static fromKey(key: string) {
		let dash = key.indexOf('-');
		return new CarKey(parseInt(key.substring(0, dash)), key.substring(dash + 1, key.length));
	}

	static fromStanding(standing: Standing) {
		return new CarKey(standing.slotID, standing.vehicleFilename);
	}

	static fromCar(car: CarModel) {
		return new CarKey(car.slotId, car.veh);
	}
}

export interface Car {
	slotId: number;
	veh: string;
	vehicleName: string;
	teamName: string;
	class: string;
	number: string;
	id: string;
}

export interface CarState {
	key: string;
	countLapFlag: string;
	driverName: string;
	finishStatus: string;
	inGarageStall: boolean;
	lapStartET: number;
	lapsCompleted: number;
	penalties: number;
	pitState: string;
	pitstops: number;
	pitting: boolean;
	position: number;
	serverScored: boolean;

	lastPitLap: number;
	lastPitTime: number;
	pitThisLap: boolean;
	lastStopLap: number;
	lastStopTime: number;
	stopThisLap: boolean;
	lastReleaseTime: number;
	stopLocation: number;
	lastExitTime: number;
	lastGarageLap: number;
	lastGarageInTime: number;
	lastGarageOutTime: number;
	garageThisLap: boolean;
	lastSwapLap: number;
	lastSwapTime: number;
	swapThisLap: boolean;
	swapLocation: number;
	startedLapInPit: boolean;
	LastLapEndPitState: string;
	ThisLapStartPitState: string;
	penaltyThisLap: boolean;
	totalPenalties: number;
	totalPits: number;
	totalStops: number;
}

export interface Entry {
	slotId: number;
	id: string;
	number: string;
	name: string;
	vehicle: string;
	members: Member[];
}

export interface Lap {
	lapNumber: number;
	totalTime: number;
	sector1: number;
	sector2: number;
	sector3: number;
	driver: string;
	isValid: boolean;
	position: number;
	penalty: boolean;
	garage: boolean;
	pit: boolean;
	fuel: number;
	virtualEnergy: number;
	lFTire: number;
	rFTire: number;
	lRTire: number;
	rRTire: number;
	finishStatus: string;
	startTime: number;
	timestamp: Date | null;
}

export interface Member {
	name: string;
	badge: string;
	nationality: string;
	isDriver: boolean;
	isEngineer: boolean;
	isAdmin: boolean;
}

export interface Pit {
	lap: number;
	pitTime: number;
	stopTime: number;
	releaseTime: number;
	exitTime: number;
	garageInTime: number;
	garageOutTime: number;
	swapTime: number;
	stopAfterLine: boolean;
	stopLocation: number;
	swap: boolean;
	swapLocation: number;
	penalty: boolean;
	fuel: number;
	virtualEnergy: number;
	lfChanged: boolean;
	lfCompound: string | null;
	lfNew: boolean;
	lfUsage: number;
	rfChanged: boolean;
	rfCompound: string | null;
	rfNew: boolean;
	rfUsage: number;
	lrChanged: boolean;
	lrCompound: string | null;
	lrNew: boolean;
	lrUsage: number;
	rrChanged: boolean;
	rrCompound: string | null;
	rrNew: boolean;
	rrUsage: number;
	previousStnumberDuration: number;
	time: number;
}

export interface SessionSummary {
	sessionId: string;
	primaryClientId: string;
	secondaryClientIds: Array<string>;
	clientCount: number;
	track: string;
	type: string;
	online: boolean;
	timestamp: Date;
	lastUpdate: Date;
	finished: boolean;
	active: boolean;
	carCount: number;
	lapCount: number;
	currentET: number;
	remaining: number;
	phase: number;
	bests: Bests;
}
