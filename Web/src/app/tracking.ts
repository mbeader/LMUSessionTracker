
export interface CarHistory {
	key: CarKey;
	car: Car;
	laps: Array<Lap>;
	lapsCompleted: number;
}

export interface CarKey {
	slotId: number;
	veh: string;
}
//export class CarKey {
//	public slotId: number;
//	public veh: string;

//	constructor(slotId: number, veh: string) {
//		this.slotId = slotId;
//		this.veh = veh;
//	}

//	matches(s: string) {
//		return s == this.id;
//	}

//	get id() {
//		return `${this.slotId}-${this.veh}`;
//	}
//}

export interface Car {
	slotId: number;
	veh: string;
	vehicleName: string;
	teamName: string;
	class: string;
	number: string;
	id: string;
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
	pit: boolean;
	fuel: number;
	virtualEnergy: number;
	lFTire: number;
	rFTire: number;
	lRTire: number;
	rRTire: number;
	finishStatus: string;
	timestamp: Date | null;
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
	remaining: number;
	phase: number;
}
