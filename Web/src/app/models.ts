
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

	entry: Entry;
	session: Session;
	laps: Array<Lap>;
}

export interface Chat {
	chatId: BigInt;
	sessionId: string;

	message: string;
	timestamp: Date;

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
	totalTime: number;
	sector1: number;
	sector2: number;
	sector3: number;
	isValid: boolean;
	position: number;
	pit: boolean;
	fuel: number;
	virtualEnergy: number;
	lFTire: number;
	rFTire: number;
	lRTire: number;
	rRTire: number;

	timestamp: Date | null;

	session: Session;
	car: Car;
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
