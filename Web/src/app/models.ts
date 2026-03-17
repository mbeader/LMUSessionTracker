
export interface Car {
	carId: BigInt;
	sessionId: string;
	entryId: BigInt | null;

	slotId: number;
	veh: string;
	vehicleName: string;
	teamName: string;
	class: string;
	number: string;
	id: string;

	lastState: CarState;
	entry: Entry;
	session: Session;
	laps: Array<Lap>;
	pits: Array<Pit>;
}

export interface CarState {
	carStateId: BigInt;
	carId: BigInt;
	sessionId: string;

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
	lastLapEndPitState: string;
	thisLapStartPitState: string;
	penaltyThisLap: boolean;
	totalPenalties: number;
	totalPits: number;
	totalStops: number;

	car: Car;
	session: Session;
}

export interface Chat {
	chatId: BigInt;
	sessionId: string;

	message: string;
	timestamp: BigInt;
	nanoseconds: BigInt;

	session: Session;
}

export interface Entry {
	entryId: BigInt;
	sessionId: string;

	slotId: number;
	id: string;
	number: string;
	name: string;
	vehicle: string;

	session: Session;
	car: Car;
	members: Array<Member>;
}

export interface Lap {
	lapId: BigInt;
	sessionId: string;
	carId: BigInt;

	lapNumber: number;
	driver: string;
	finishStatus: string;
	startTime: number;
	totalTime: number;
	sector1: number;
	sector2: number;
	sector3: number;
	isValid: boolean;
	position: number;
	penalty: boolean;
	garage: boolean;
	pit: boolean;
	fuel: number;
	virtualEnergy: number;
	lfUsage: number;
	rfUsage: number;
	lrUsage: number;
	rrUsage: number;
	resolved: boolean;

	timestamp: Date | null;

	session: Session;
	car: Car;

	known: boolean;
}

export interface Member {
	memberId: BigInt;
	entryId: BigInt;
	sessionId: string;

	name: string;
	badge: string;
	nationality: string;
	isDriver: boolean;
	isEngineer: boolean;
	isAdmin: boolean;

	entry: Entry;
	session: Session;
}

export interface Pit {
	pitId: BigInt;
	sessionId: string;
	carId: BigInt;

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
	lfCompound: string;
	lfNew: boolean;
	lfUsage: number;
	rfChanged: boolean;
	rfCompound: string;
	rfNew: boolean;
	rfUsage: number;
	lrChanged: boolean;
	lrCompound: string;
	lrNew: boolean;
	lrUsage: number;
	rrChanged: boolean;
	rrCompound: string;
	rrNew: boolean;
	rrUsage: number;
	previousStintDuration: number;
	time: number;
	resolved: boolean;

	session: Session;
	car: Car;
}

export interface SessionState {
	sessionStateId: BigInt;
	sessionId: string;

	timestamp: Date;

	ambientTemp: number;
	averagePathWetness: number;
	currentEventTime: number;
	darkCloud: number;
	gamePhase: number | null;
	inRealtime: boolean | null;
	maxPathWetness: number;
	minPathWetness: number;
	numRedLights: number | null;
	numberOfPlayers: number | null;
	numberOfVehicles: number | null;
	raceCompletion: number | null;
	raining: number | null;
	sector1Flag: string;
	sector2Flag: string;
	sector3Flag: string;
	startLightFrame: number | null;
	timeRemainingInGamePhase: number | null;
	trackTemp: number | null;
	windVelocity: number | null;
	windX: number | null;
	windY: number | null;
	windZ: number | null;
	yellowFlagState: string;

	session: Session;
}

export interface Session {
	sessionId: string;

	timestamp: Date;
	isOnline: boolean;
	isClosed: boolean;

	endEventTime: number;
	gameMode: string;
	lapDistance: number;
	maxPlayers: number;
	maxTime: number;
	maximumLaps: number;
	passwordProtected: boolean;
	serverName: string;
	serverPort: number;
	sessionType: string;
	startEventTime: number;
	trackName: string;

	lastState: SessionState;
	cars: Array<Car>;
	laps: Array<Lap>;
	entries: Array<Entry>;
	members: Array<Member>;
	chats: Array<Chat>;
}
