import { Component, inject, Injectable } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { NgbPagination } from '@ng-bootstrap/ng-bootstrap/pagination';
import { BehaviorSubject, debounceTime, delay, Observable, Subject, switchMap, tap } from 'rxjs';
import { ServerApiService } from '../server-api.service';
import { ServerLiveService } from '../server-live.service';
import { Format } from '../format';
import { SessionSummary } from '../tracking';
import { IndexViewModel } from '../view-models';

@Component({
	selector: 'app-sessions',
	imports: [RouterLink, NgbPagination, AsyncPipe],
	templateUrl: './sessions.html',
	styleUrl: './sessions.css',
})
export class Sessions {
	private initCount: number = 0;
	private _liveSessions = new BehaviorSubject<SessionSummary[]>([]);
	private api = inject(ServerApiService);
	private live = inject(ServerLiveService);
	service = inject(SessionsService);
	liveSessions: Observable<SessionSummary[]>;
	sessions: Observable<SessionSummary[]>;
	total: Observable<number>;
	now: Date = new Date();
	init: boolean = true;
	Format = Format;

	constructor() {
		this.liveSessions = this._liveSessions.asObservable();
		this.api.getLiveSessions().subscribe((result) => {
			this._liveSessions.next(result.sessions ?? []);
			this.live.joinSessions(liveSessions => this._liveSessions.next(liveSessions));
		});
		this.sessions = this.service.sessions;
		this.total = this.service.total;
		this.sessions.subscribe(() => { if (this.init && this.initCount++ > 0) this.init = false; });
		this.whenExists('#all-tab', tab => {
			tab.addEventListener('shown.bs.tab', event => {
				this.init = true;
				this.initCount = 0;
				this.service.reset();
			});
		});
	}

	private whenExists<T extends Element>(selector: string, callback: (el: T) => void) {
		new Promise<T>((resolve) => {
			let fn = (fn: any) => {
				let el = document.querySelector<T>(selector);
				if (!el)
					setTimeout(fn, 100);
				else
					resolve(el);
			}
			fn(fn);
		}).then(callback);
	}
}

interface State {
	page: number;
	pageSize: number;
}

@Injectable({ providedIn: 'root' })
export class SessionsService {
	private api = inject(ServerApiService);
	private _loading = new BehaviorSubject<boolean>(true);
	private _search = new Subject<void>();
	private _sessions = new BehaviorSubject<SessionSummary[]>([]);
	private _total = new BehaviorSubject<number>(0);

	private _state: State = {
		page: 1,
		pageSize: 20
	};

	constructor() {
		this._search
			.pipe(
				tap(() => this._loading.next(true)),
				debounceTime(200),
				switchMap(() => this.doSearch(this.api)),
				delay(200),
				tap(() => this._loading.next(false)),
			)
			.subscribe((result) => {
				this._sessions.next(result.sessions ?? []);
				this._total.next(result.total);
			});
		this._search.next();
	}

	reset() {
		this._sessions.next([]);
		this._total.next(0);
		this._search.next();
	}

	get sessions() {
		return this._sessions.asObservable();
	}
	get total() {
		return this._total.asObservable();
	}
	get loading() {
		return this._loading.asObservable();
	}
	get page() {
		return this._state.page;
	}
	get pageSize() {
		return this._state.pageSize;
	}

	set page(page: number) {
		this._set({ page });
	}
	set pageSize(pageSize: number) {
		this._set({ pageSize });
	}

	private _set(patch: Partial<State>) {
		Object.assign(this._state, patch);
		this._search.next();
	}

	private doSearch(api: ServerApiService): Observable<IndexViewModel> {
		const { pageSize, page } = this._state;

		return api.getSessions(page, pageSize);
	}
}
