import { SessionInfo, Standing } from './lmu';
import { Session, SessionState } from './models';
import { SessionSummary, CarHistory, Car } from './tracking';

export class BestLapsFilters {
	public track: string | null = null;
	public since: string | null = null;
	public network: string | null = null;
	public classes: string[] | null = null;
	public sessions: string[] | null = null;
	public knownDriversOnly: boolean = false;
}

export class IndexViewModel {
	public sessions: SessionSummary[] | null = null;
	public total: number = 0;
}

export class LapsViewModel {
	public car: CarHistory | null = null;
	public session: SessionSummary | null = null;

	static merge(vm: LapsViewModel, other: LapsViewModel) {
		vm.car = other.car;
	}
}

export class SessionViewModel {
	public info: SessionInfo | null = null;
	public standings: Array<Standing> | null = null;
	public history: Array<CarHistory> | null = null;
	public positionInClass: { [key: string]: number } | null = null;
	public entries: { [key: string]: Car } | null = null;

	public session: Session | null = null;
	public sessionState: SessionState | null = null;

	static merge(vm: SessionViewModel, other: SessionViewModel) {
		vm.info = other.info;
		vm.standings = other.standings;
		vm.history = other.history;
		vm.positionInClass = other.positionInClass;
		vm.entries = other.entries;
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
