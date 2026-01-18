import { Standing } from "./lmu";
import { Car as CarModel } from "./models";

export interface CarHistory {
	/** CarKey */
	key: string;
	car: Car;
	laps: Array<Lap | null>;
	lapsCompleted: number;
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

export interface Member {
	name: string;
	badge: string;
	nationality: string;
	isDriver: boolean;
	isEngineer: boolean;
	isAdmin: boolean;
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
