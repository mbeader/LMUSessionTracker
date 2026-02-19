import { SessionInfo, Standing } from './lmu';
import { BestLap, ClassBest, Lap, Session, SessionState } from './models';
import { SessionSummary, CarHistory, Car, Bests, CarState } from './tracking';

export class AboutOptions {
	public repoUrl: string | null = null;
	public releasesUrl: string | null = null;
	public binaryUrl: string | null = null;

	static hasUrl(url: string | null) {
		return url && url.trim() ? true : false;
	}

	static hasAnyUrl(options: AboutOptions) {
		return this.hasUrl(options.repoUrl) || this.hasUrl(options.releasesUrl) || this.hasUrl(options.binaryUrl);
	}
}

export class BestLapsFilters {
	public track: string | null = null;
	public since: string | null = null;
	public network: string | null = null;
	public classes: string[] | null = null;
	public sessions: string[] | null = null;
	public knownDriversOnly: boolean = false;
}

export class BestLapsViewModel {
	public laps: BestLap[] | null = null;
	public classBests: { [key: string]: ClassBest } | null = null;
}

export class IndexViewModel {
	public sessions: SessionSummary[] | null = null;
	public total: number = 0;
}

export class LapsViewModel {
	public car: CarHistory | null = null;
	public session: SessionSummary | null = null;
	public bests: Bests | null = null;

	static merge(vm: LapsViewModel, other: LapsViewModel) {
		vm.car = other.car;
		vm.bests = other.bests;
	}
}

export class SessionViewModel {
	public info: SessionInfo | null = null;
	public standings: Array<Standing> | null = null;
	public history: Array<CarHistory> | null = null;
	public carState: Array<CarState> | null = null;
	public positionInClass: { [key: string]: number } | null = null;
	public entries: { [key: string]: Car } | null = null;
	public results: Lap[] | null = null;
	public bests: Bests | null = null;

	public session: Session | null = null;
	public sessionState: SessionState | null = null;
	public nextSession: Session | null = null;

	static merge(vm: SessionViewModel, other: SessionViewModel) {
		vm.info = other.info;
		vm.standings = other.standings;
		vm.history = other.history;
		vm.carState = other.carState;
		vm.positionInClass = other.positionInClass;
		vm.entries = other.entries;
		vm.bests = other.bests;
		if (other.info) {
			let session: any = {}, sessionState: any = {};
			session.endEventTime = other.info.endEventTime;
			session.maxTime = other.info.maxTime;
			session.sessionType = other.info.session;
			session.startEventTime = other.info.startEventTime;
			session.trackName = other.info.trackName;
			sessionState.ambientTemp = other.info.ambientTemp;
			sessionState.averagePathWetness = other.info.averagePathWetness;
			sessionState.currentEventTime = other.info.currentEventTime;
			sessionState.gamePhase = other.info.gamePhase;
			sessionState.maxPathWetness = other.info.maxPathWetness;
			sessionState.minPathWetness = other.info.minPathWetness;
			sessionState.numberOfPlayers = other.info.numberOfPlayers;
			sessionState.raceCompletion = other.info.raceCompletion.timeCompletion
			sessionState.raining = other.info.raining;
			sessionState.sector1Flag = other.info.sectorFlag[0];
			sessionState.sector2Flag = other.info.sectorFlag[1];
			sessionState.sector3Flag = other.info.sectorFlag[2];
			sessionState.timeRemainingInGamePhase = other.info.timeRemainingInGamePhase;
			sessionState.trackTemp = other.info.trackTemp;
			sessionState.yellowFlagState = other.info.yellowFlagState;
			session.lapDistance = vm.session?.lapDistance;
			vm.session = session as Session;
			vm.sessionState = sessionState as SessionState;
			vm.nextSession = other.nextSession;
		}
	}

	static flagClass(sectorFlag: string, even?: boolean) {
		switch (sectorFlag) {
			case 'UNKNOWN':
				return 'pe-3 bg-success';
			case 'YELLOW':
				return 'pe-3 bg-warning';
			case 'RED':
				return 'pe-3 bg-danger';
			case 'CHECKERED':
				return even ? 'pe-3 bg-white' : 'pe-3 bg-black';
			default:
				return 'pe-3 bg-secondary';
		}
	}
}

export class JoinRequest {
	public readonly type: string;
	public readonly sessionId: string | null;
	public readonly key: string | null;

	constructor(type: string, sessionId?: string, key?: string) {
		this.type = type;
		this.sessionId = sessionId ?? null;
		this.key = key ?? null;
	}
};

export class Point2D {
	x: number = 0;
	y: number = 0;
}

export class TrackMap {
	points: Point2D[] = [];
	pits: Point2D[] = [];
	s1: Point2D[] = [];
	s2: Point2D[] = [];
	s3: Point2D[] = [];
	maxX: number = 0;
	maxY: number = 0;
	minX: number = 0;
	minY: number = 0;

	constructor(map?: TrackMap) {
		if (map) {
			this.points = map.points ?? this.points;
			this.pits = map.pits ?? this.pits;
			this.s1 = map.s1 ?? this.s1;
			this.s2 = map.s2 ?? this.s2;
			this.s3 = map.s3 ?? this.s3;
			this.maxX = map.maxX;
			this.maxY = map.maxY;
			this.minX = map.minX;
			this.minY = map.minY;
		}
	}

	get length() {
		return this.hasSectors() ? this.s1.length + this.s2.length + this.s3.length : this.points.length;
	}

	get maxx() { return this.maxX; }
	get maxy() { return this.maxY; }
	get minx() { return this.minX; }
	get miny() { return this.minY; }

	set maxx(v: number) { this.maxX = v; }
	set maxy(v: number) { this.maxY = v; }
	set minx(v: number) { this.minX = v; }
	set miny(v: number) { this.minY = v; }

	hasSectors() {
		return this.s1.length > 0 && this.s2.length > 0 && this.s3.length > 0;
	}
}

export class SessionTransitionViewModel {
	public sessionId: string | null = null;
	public info: SessionInfo | null = null;
}
