export interface AttackMode {
	remainingCount: number;
	timeRemaining: number;
	totalCount: number;
}

export interface Chat {
	message: string;
	timestamp: BigInt;
}

export interface Completion {
	timeCompletion: number;
}

export enum GamePhase {
	Starting = 0,
	ReconnaissanceLaps = 1,
	Grid = 2,
	FormationLap = 3,
	Countdown = 4,
	Green = 5,
	FCY = 6,
	SessionStopped = 7,
	Checkered = 8,
	Paused = 9,
}

export interface GameState {
	MultiStintState: string;
	PitEntryDist: number | null;
	PitState: string;
	closeestWeatherNode: WeatherNode;
	gamePhase: string;
	haveILoadedAllMyTiresYet: boolean;
	inControlOfVehicle: boolean;
	inMonitor: boolean;
	isReplayActive: boolean;
	playerVehicleLoaded: boolean;
	raceFinished: boolean;
	teamVehicleState: string;
	timeOfDay: number;
}

export interface MultiplayerDriver {
	badge: string;
	isConnected: boolean;
	nationality: string;
	roles: Array<string>;
	teamId: string;
	teamName: string;
	uniqueTeamId: string;
}

export interface MultiplayerTeam {
	Id: string;
	carNumber: string;
	drivers: Map<string, MultiplayerTeamMember>;
	name: string;
	vehicle: string;
}

export interface MultiplayerTeamMember {
	badge: string;
	nationality: string;
	roles: Array<string>;
}

export interface MultiplayerTeams {
	coherenceId: number;
	drivers: Map<string, MultiplayerDriver>;
	teams: Map<string, MultiplayerTeam>;
}

export interface Position {
	type: number;
	x: number;
	y: number;
	z: number;
}

export interface ProfileInfo {
	language: string;
	name: string;
	nationality: string;
	nick: string;
	steamID: string;
}

export interface ScheduledSession {
	airTemp: number;
	lengthTime: number;
	name: string;
	rainChance: number;
}

export interface SessionInfo {
	ambientTemp: number;
	averagePathWetness: number;
	currentEventTime: number;
	darkCloud: number;
	endEventTime: number;
	gameMode: string;
	gamePhase: number;
	inRealtime: boolean;
	lapDistance: number;
	maxPathWetness: number;
	maxPlayers: number;
	maxTime: number;
	maximumLaps: number;
	minPathWetness: number;
	numRedLights: number;
	numberOfPlayers: number;
	numberOfVehicles: number;
	passwordProtected: boolean;
	playerFileName: string;
	playerName: string;
	raceCompletion: Completion;
	raining: number;
	sectorFlag: Array<string>;
	serverName: string;
	serverPort: number;
	session: string;
	startEventTime: number;
	startLightFrame: number;
	timeRemainingInGamePhase: number;
	trackName: string;
	trackTemp: number;
	windSpeed: Velocity;
	yellowFlagState: string;
}

export interface SessionsInfoForEvent {
	scheduledSessions: Array<ScheduledSession>;
}

export interface Standing {
	attackMode: AttackMode;
	bestLapSectorTime1: number;
	bestLapSectorTime2: number;
	bestLapTime: number;
	bestSectorTime1: number;
	bestSectorTime2: number;
	carAcceleration: Velocity;
	carClass: string;
	carId: string;
	carNumber: string;
	carPosition: Position;
	carVelocity: Velocity;
	countLapFlag: string;
	currentSectorTime1: number;
	currentSectorTime2: number;
	driverName: string;
	drsActive: boolean;
	estimatedLapTime: number;
	finishStatus: string;
	flag: string;
	focus: boolean;
	fuelFraction: number;
	fullTeamName: string;
	gamePhase: string;
	hasFocus: boolean;
	headlights: boolean;
	inControl: number;
	inGarageStall: boolean;
	lapDistance: number;
	lapStartET: number;
	lapsBehindLeader: number;
	lapsBehindNext: number;
	lapsCompleted: number;
	lastLapTime: number;
	lastSectorTime1: number;
	lastSectorTime2: number;
	pathLateral: number;
	penalties: number;
	pitGroup: string;
	pitLapDistance: number;
	pitState: string;
	pitstops: number;
	pitting: boolean;
	player: boolean;
	position: number;
	qualification: number;
	sector: string;
	serverScored: boolean;
	slotID: number;
	steamID: BigInt;
	timeBehindLeader: number;
	timeBehindNext: number;
	timeIntoLap: number;
	trackEdge: number;
	underYellow: boolean;
	upgradePack: string;
	vehicleFilename: string;
	vehicleName: string;
}

export interface StandingsHistory extends Map<string, Array<StandingsHistoryLap>> {

}

export interface StandingsHistoryLap {
	carClass: string;
	driverName: string;
	finishStatus: string;
	lapTime: number;
	pitting: boolean;
	position: number;
	sectorTime1: number;
	sectorTime2: number;
	slotID: number;
	totalLaps: number;
	vehicleName: string;
}

export interface Strategy {
	driver: string;
	driverSwap: boolean;
	fuel: number | null;
	lap: number;
	penalty: boolean;
	previousStintDuration: number;
	time: number;
	tyres: StrategyTires;
	ve: number;
}

export interface StrategyTire {
	changed: boolean;
	compound: string;
	New: boolean;
	usage: number | null;
}

export interface StrategyTires {
	fl: StrategyTire;
	fr: StrategyTire;
	rl: StrategyTire;
	rr: StrategyTire;
}

export interface StrategyUsage extends Map<string, Array<StrategyDriverUsage>> {
}

export interface StrategyDriverUsage {
	/// <summary>
	/// Player only
	/// </summary>
	fuel: number;
	lap: number;
	pit: boolean;
	stint: number;
	/// <summary>
	/// Player only
	/// </summary>
	tyres: Array<number>;
	ve: number;
}

export interface TeamStrategy {
	// [name, strategy]
	Name: string;
	Strategy: Array<Strategy>;
}

export interface TrackMapPoint {
	x: number;
	y: number; // up
	z: number;
	/// <summary>
	/// 0 - track
	/// 1 - pit/paddock (including entry+exit)
	/// >=3 - grid (pair of points)
	/// >=107 - pit (pair of points)
	/// </summary>
	type: number;
}

export interface Velocity {
	velocity: number;
	x: number;
	y: number;
	z: number;
}

export interface WeatherNode {
	Duration: number;
	Humidity: number;
	RainChance: number;
	Sky: number;
	StartTime: number;
	Temperature: number;
	WindDirection: number;
	WindSpeed: number;
}
