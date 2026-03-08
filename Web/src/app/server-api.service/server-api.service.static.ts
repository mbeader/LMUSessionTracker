import { Injectable, InjectionToken } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { IndexViewModel, SessionViewModel, LapsViewModel, BestLapsFilters, BestLapsViewModel, AboutOptions, TrackMap, ChatViewModel, ChatMessage, Result } from '../view-models';
import { Car, CarState, Chat, Entry, Lap, Member, Pit, Session, SessionState } from '../models';
import { Car as TCar, CarState as TCarState, CarKey, CarHistory, Lap as TLap, Pit as TPit, SessionSummary } from '../tracking';
import initSqlJs, { Database } from 'sql.js';

const sqlPromise = initSqlJs({ locateFile: file => `/${file}` });
const dataPromise = fetch('sample.db').then(res => res.arrayBuffer());
const [SQL, buf] = await Promise.all([sqlPromise, dataPromise]);
const db = buf.byteLength == 0 ? null : new SQL.Database(new Uint8Array(buf));
console.log(db ? 'Loaded static db' : 'No static db found');

export const ServerApiServiceToken = new InjectionToken<ServerApiService>('server api');

export interface ServerApiService {
	getSessions(page: number, pageSize: number): Observable<IndexViewModel>;
	getLiveSessions(): Observable<IndexViewModel>;
	getSession(sessionId: string): Promise<SessionViewModel>;
	getLaps(sessionId: string, carId: string): Promise<LapsViewModel>;
	getEntryList(sessionId: string): Promise<Car[]>;
	getTrackMap(sessionId: string): Promise<TrackMap>;
	getChat(sessionId: string): Promise<ChatViewModel>;
	getTracks(): Promise<string[]>;
	getBestLaps(filters: BestLapsFilters): Promise<BestLapsViewModel>;
	getAbout(): Observable<AboutOptions>;
}

@Injectable({
	providedIn: 'root',
})
export class HttpServerApiService implements ServerApiService {
	getSessions(page: number, pageSize: number): Observable<IndexViewModel> {
		throw new Error('Method not implemented.');
	}
	getLiveSessions(): Observable<IndexViewModel> {
		throw new Error('Method not implemented.');
	}
	getSession(sessionId: string): Promise<SessionViewModel> {
		throw new Error('Method not implemented.');
	}
	getLaps(sessionId: string, carId: string): Promise<LapsViewModel> {
		throw new Error('Method not implemented.');
	}
	getEntryList(sessionId: string): Promise<Car[]> {
		throw new Error('Method not implemented.');
	}
	getTrackMap(sessionId: string): Promise<TrackMap> {
		throw new Error('Method not implemented.');
	}
	getChat(sessionId: string): Promise<ChatViewModel> {
		throw new Error('Method not implemented.');
	}
	getTracks(): Promise<string[]> {
		throw new Error('Method not implemented.');
	}
	getBestLaps(filters: BestLapsFilters): Promise<BestLapsViewModel> {
		throw new Error('Method not implemented.');
	}
	getAbout(): Observable<AboutOptions> {
		throw new Error('Method not implemented.');
	}
}

@Injectable({
	providedIn: 'root',
})
export class StaticServerApiService implements ServerApiService {
	private repo = new Repository();
	private summaries!: SessionSummary[];
	private histories!: { [key: string]: CarHistory[] };
	private results!: { [key: string]: Result[] };

	constructor() {
		this.loadSummaries();
		this.loadHistory();
		this.loadResults();
	}

	private delay(callback: () => void) {
		setTimeout(callback, 100);
	}

	private loadSummaries() {
		if (!this.summaries) {
			this.summaries = this.repo.getSessions().map(x => {
				let summary = {} as SessionSummary;
				summary.sessionId = x.sessionId;
				//summary.primaryClientId = x.primaryClientId;
				summary.secondaryClientIds = [];
				summary.clientCount = 0;
				summary.track = x.trackName;
				summary.type = x.sessionType;
				summary.online = x.isOnline;
				summary.timestamp = x.timestamp;
				summary.lastUpdate = x.lastState.timestamp;
				summary.finished = x.isClosed;
				summary.active = !x.isClosed;
				summary.carCount = x.cars.length;
				summary.lapCount = x.cars.map(x => x.laps.length).reduce((prev, curr) => prev + curr, 0);
				summary.currentET = x.lastState.currentEventTime;
				summary.remaining = x.lastState.timeRemainingInGamePhase ?? -1;
				summary.phase = x.lastState.gamePhase ?? 0;
				//summary.bests = x.bests;
				return summary;
			});
		}
	}

	private loadHistory() {
		if (!this.histories) {
			this.histories = {};
			for (let session of this.repo.getSessions()) {
				let history = [];
				for (let car of session.cars) {
					let ch = {} as CarHistory;
					ch.car = {
						slotId: car.slotId,
						veh: car.veh,
						vehicleName: car.vehicleName,
						teamName: car.teamName,
						class: car.class,
						number: car.number,
						id: car.id,
					} as TCar;
					ch.key = CarKey.fromCar(ch.car).id;
					ch.laps = car.laps.map(x => {
						let lap = {} as TLap;
						lap.lapNumber = x.lapNumber;
						lap.totalTime = x.totalTime;
						lap.sector1 = x.sector1;
						lap.sector2 = x.sector2;
						lap.sector3 = x.sector3;
						lap.driver = x.driver;
						lap.isValid = x.isValid;
						lap.position = x.position;
						lap.penalty = x.penalty;
						lap.garage = x.garage;
						lap.pit = x.pit;
						lap.fuel = x.fuel;
						lap.virtualEnergy = x.virtualEnergy;
						lap.lFTire = x.lFTire;
						lap.rFTire = x.rFTire;
						lap.lRTire = x.lRTire;
						lap.rRTire = x.rRTire;
						lap.finishStatus = x.finishStatus;
						lap.startTime = x.startTime;
						lap.timestamp = x.timestamp;
						return lap;
					});
					ch.pits = car.pits.map(x => {
						let pit = {} as TPit;
						pit.lap = x.lap;
						pit.pitTime = x.pitTime;
						pit.stopTime = x.stopTime;
						pit.releaseTime = x.releaseTime;
						pit.exitTime = x.exitTime;
						pit.garageInTime = x.garageInTime;
						pit.garageOutTime = x.garageOutTime;
						pit.swapTime = x.swapTime;
						pit.stopAfterLine = x.stopAfterLine;
						pit.stopLocation = x.stopLocation;
						pit.swap = x.swap;
						pit.swapLocation = x.swapLocation;
						pit.penalty = x.penalty;
						pit.fuel = x.fuel;
						pit.virtualEnergy = x.virtualEnergy;
						pit.lfChanged = x.lfChanged;
						pit.lfCompound = x.lfCompound;
						pit.lfNew = x.lfNew;
						pit.lfUsage = x.lfUsage;
						pit.rfChanged = x.rfChanged;
						pit.rfCompound = x.rfCompound;
						pit.rfNew = x.rfNew;
						pit.rfUsage = x.rfUsage;
						pit.lrChanged = x.lrChanged;
						pit.lrCompound = x.lrCompound;
						pit.lrNew = x.lrNew;
						pit.lrUsage = x.lrUsage;
						pit.rrChanged = x.rrChanged;
						pit.rrCompound = x.rrCompound;
						pit.rrNew = x.rrNew;
						pit.rrUsage = x.rrUsage;
						pit.previousStintDuration = x.previousStintDuration;
						pit.time = x.time;
						return pit;
					});
					ch.lapsCompleted = ch.laps.length;
					history.push(ch);
				}
				this.histories[session.sessionId] = history;
			}
		}
	}

	private loadResults() {
		if (!this.results) {
			this.results = {};
			for (let session of this.repo.getSessions()) {
				let results = [];
				for (let car of session.cars) {
					let result = new Result();
					let key = new CarKey(car.slotId, car.veh).id;
					let history = this.histories[session.sessionId].find(x => x.key == key);
					if (history) {
						result.car = history.car;
						result.lastLap = (history.laps.length > 0 ? history.laps[history.laps.length - 1] : undefined) ?? undefined;
						result.bestLap = history.laps.reduce((prev: TLap | null, curr: TLap | null) => {
							if (curr && curr.isValid && curr.totalTime >= 0)
								return !prev || curr.totalTime < prev.totalTime ? curr : prev;
							return prev;
						}, null) ?? undefined;
						let state = session.cars.find(x => new CarKey(x.slotId, x.veh).id == key)?.lastState;
						result.carState = {} as TCarState;
						result.carState.key = key;
						if (state) {
							result.carState.countLapFlag = state.countLapFlag;
							result.carState.driverName = state.driverName;
							result.carState.finishStatus = state.finishStatus;
							result.carState.inGarageStall = state.inGarageStall;
							result.carState.lapStartET = state.lapStartET;
							result.carState.lapsCompleted = state.lapsCompleted;
							result.carState.penalties = state.penalties;
							result.carState.pitState = state.pitState;
							result.carState.pitstops = state.pitstops;
							result.carState.pitting = state.pitting;
							result.carState.position = state.position;
							result.carState.serverScored = state.serverScored;
							result.carState.lastPitLap = state.lastPitLap;
							result.carState.lastPitTime = state.lastPitTime;
							result.carState.pitThisLap = state.pitThisLap;
							result.carState.lastStopLap = state.lastStopLap;
							result.carState.lastStopTime = state.lastStopTime;
							result.carState.stopThisLap = state.stopThisLap;
							result.carState.lastReleaseTime = state.lastReleaseTime;
							result.carState.stopLocation = state.stopLocation;
							result.carState.lastExitTime = state.lastExitTime;
							result.carState.lastGarageLap = state.lastGarageLap;
							result.carState.lastGarageInTime = state.lastGarageInTime;
							result.carState.lastGarageOutTime = state.lastGarageOutTime;
							result.carState.garageThisLap = state.garageThisLap;
							result.carState.lastSwapLap = state.lastSwapLap;
							result.carState.lastSwapTime = state.lastSwapTime;
							result.carState.swapThisLap = state.swapThisLap;
							result.carState.swapLocation = state.swapLocation;
							result.carState.startedLapInPit = state.lastLapEndPitState == 'ENTERING' || state.lastLapEndPitState == 'STOPPED';
							result.carState.lastLapEndPitState = state.lastLapEndPitState;
							result.carState.thisLapStartPitState = state.thisLapStartPitState;
							result.carState.penaltyThisLap = state.penaltyThisLap;
							result.carState.totalPenalties = state.totalPenalties;
							result.carState.totalPits = state.totalPits;
							result.carState.totalStops = state.totalStops;
						}
					}
					results.push(result);
				}
				results.sort((a, b) => {
					let aPos = a.carState?.position ?? Number.MAX_SAFE_INTEGER;
					let bPos = b.carState?.position ?? Number.MAX_SAFE_INTEGER;
					return aPos < bPos ? -1 : aPos > bPos ? 1 : a.car.slotId - b.car.slotId;
				});
				this.results[session.sessionId] = results;
			}
		}
	}

	getSessions(page: number, pageSize: number): Observable<IndexViewModel> {
		let sessions = this.summaries;
		let total = Math.ceil(sessions.length / pageSize);
		let start = (page - 1) * pageSize;
		if (start < sessions.length)
			sessions = sessions.slice(start, start + pageSize);
		else
			sessions = [];
		return new Observable(subscriber => this.delay(() => subscriber.next(Object.assign(new IndexViewModel(), { sessions: sessions, total: total }))));
	}

	getLiveSessions(): Observable<IndexViewModel> {
		return new Observable(subscriber => this.delay(() => subscriber.next(Object.assign(new IndexViewModel(), { sessions: [] }))));
	}

	getSession(sessionId: string): Promise<SessionViewModel> {
		let session = this.repo.getSession(sessionId);
		let vm = new SessionViewModel();
		if (session) {
			vm.session = session;
			vm.sessionState = session.lastState;
			vm.history = this.histories[sessionId];
			vm.results = this.results[sessionId];
			vm.positionInClass = {};
			let classes: { [key: string]: number } = {};
			for (let car of vm.results) {
				if (!classes[car.car.class]) {
					classes[car.car.class] = 1;
				}
				vm.positionInClass[CarKey.fromCar(car.car).id] = classes[car.car.class];
				classes[car.car.class]++;
			}
		}
		return new Promise(resolve => resolve(vm));
	}

	getLaps(sessionId: string, carId: string): Promise<LapsViewModel> {
		let vm = new LapsViewModel();
		vm.session = this.summaries.find(x => x.sessionId == sessionId) ?? null;
		vm.car = this.histories[sessionId].find(x => x.key == carId) ?? null;
		vm.currentET = vm.session?.currentET ?? 0;
		return new Promise(resolve => resolve(vm));
	}

	getEntryList(sessionId: string): Promise<Car[]> {
		return new Promise(resolve => resolve(this.repo.getCars(sessionId)));
	}

	getTrackMap(sessionId: string): Promise<TrackMap> {
		return new Promise(resolve => resolve(new TrackMap()));
	}

	getChat(sessionId: string): Promise<ChatViewModel> {
		let session = this.repo.getSession(sessionId);
		let vm = new ChatViewModel();
		vm.chat = session?.chats.map(x => {
			let chat = new ChatMessage();
			chat.message = x.message;
			chat.timestamp = Number(x.timestamp) * 1000 + Number(x.nanoseconds) / 1000000;
			return chat;
		}) ?? [];
		vm.append = false;
		return new Promise(resolve => resolve(vm));
	}

	getTracks(): Promise<string[]> {
		return new Promise(resolve => resolve([]));
	}

	getBestLaps(filters: BestLapsFilters): Promise<BestLapsViewModel> {
		return new Promise(resolve => resolve(new BestLapsViewModel()));
	}

	getAbout(): Observable<AboutOptions> {
		return new Observable(subscriber => { subscriber.next(Object.assign(new AboutOptions(), { repoUrl: environment.repoUrl })); });
	}
}

class Repository {
	private sessions!: Session[];

	constructor() {
		if (db) {
			this.loadSessions(db);
		} else {
			this.sessions = [];
		}
	}

	private mapColumns<T>(row: initSqlJs.ParamsObject) {
		let obj: any = {};
		for (let col in row) {
			let colName = col[0].toLowerCase() + col.substring(1);
			obj[colName] = row[col];
		}
		return obj as T;
	}

	private loadSessions(db: Database) {
		if (!this.sessions) {
			const query = db.prepare('SELECT * FROM Sessions');
			const stateQuery = db.prepare('SELECT * FROM SessionStates WHERE SessionId = $sessionId');
			const chatQuery = db.prepare('SELECT * FROM Chats WHERE SessionId = $sessionId');
			this.sessions = [];
			while (query.step()) {
				let session = this.mapColumns<Session>(query.getAsObject());
				let sessionState = this.mapColumns<SessionState>(stateQuery.getAsObject({ $sessionId: session.sessionId }));
				session.lastState = sessionState;
				session.chats = [];
				chatQuery.bind({ $sessionId: session.sessionId });
				while (chatQuery.step()) {
					let chat = this.mapColumns<Chat>(chatQuery.getAsObject());
					session.chats.push(chat);
				}
				session.cars = this.loadCars(db, session.sessionId);
				this.sessions.push(session);
			}
		}
	}

	private loadCars(db: Database, sessionId: string) {
		const carQuery = db.prepare('SELECT * FROM Cars WHERE SessionId = $sessionId');
		const entryQuery = db.prepare('SELECT * FROM Entries WHERE SessionId = $sessionId AND EntryId = $entryId');
		const memberQuery = db.prepare('SELECT * FROM Members WHERE SessionId = $sessionId AND EntryId = $entryId');
		const lapsQuery = db.prepare('SELECT * FROM Laps WHERE SessionId = $sessionId AND CarId = $carId');
		const pitsQuery = db.prepare('SELECT * FROM Pits WHERE SessionId = $sessionId AND CarId = $carId');
		const stateQuery = db.prepare('SELECT * FROM CarStates WHERE SessionId = $sessionId AND CarId = $carId');
		carQuery.bind({ $sessionId: sessionId });
		let cars = [];
		while (carQuery.step()) {
			let car = this.mapColumns<Car>(carQuery.getAsObject());
			let entry = this.mapColumns<Entry>(entryQuery.getAsObject({ $sessionId: sessionId, $entryId: Number(car.entryId) }));
			car.entry = entry;
			entry.members = [];
			memberQuery.bind({ $sessionId: sessionId, $entryId: Number(car.entryId) });
			while (memberQuery.step()) {
				let member = this.mapColumns<Member>(memberQuery.getAsObject());
				entry.members.push(member);
			}
			car.laps = [];
			lapsQuery.bind({ $sessionId: sessionId, $carId: Number(car.carId) });
			while (lapsQuery.step()) {
				let lap = this.mapColumns<Lap>(lapsQuery.getAsObject());
				car.laps.push(lap);
			}
			car.pits = [];
			pitsQuery.bind({ $sessionId: sessionId, $carId: Number(car.carId) });
			while (pitsQuery.step()) {
				let pit = this.mapColumns<Pit>(pitsQuery.getAsObject());
				car.pits.push(pit);
			}
			car.lastState = this.mapColumns<CarState>(stateQuery.getAsObject({ $sessionId: sessionId, $carId: Number(car.carId) }));
			cars.push(car);
		}
		return cars;
	}

	getSessions() {
		//this.loadSessions();
		let sessions = this.sessions.slice();// ?? [];
		sessions.reverse();
		return sessions;
	}

	getSession(sessionId: string) {
		//this.loadSessions();
		return this.sessions.find(x => x.sessionId == sessionId);
	}

	getCars(sessionId: string) {
		return this.getSession(sessionId)?.cars ?? [];
	}
}
