import { SessionInfo, Standing } from './lmu';
import { Session, SessionState } from './models';
import { SessionSummary, CarHistory, Car } from './tracking';

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
	public positionInClass: { [key: string]: number } | null = null;
	public entries: { [key: string]: Car } | null = null;

	public session: Session | null = null;
	public sessionState: SessionState | null = null;
}
