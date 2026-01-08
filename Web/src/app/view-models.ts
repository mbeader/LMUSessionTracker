import { SessionInfo, Standing } from './lmu';
import { Session } from './models';
import { SessionSummary, CarHistory, Car, CarKey } from './tracking';

export class IndexViewModel {
	public sessions: Array<SessionSummary> | null = null;
}

export class LapsViewModel {
	public car: CarHistory | null = null;
	public session: SessionSummary | null = null;
}

export class SessionViewModel {
	public info: SessionInfo | null = null;
	public standings: Array<Standing> | null = null;
	public history: Array<CarHistory> | null = null;
	public positionInClass: Map<CarKey, number> = new Map();
	public entries: Map<CarKey, Car> = new Map();

	public session: Session | null = null;
	get sessionState() { return this.session?.lastState; }
}
