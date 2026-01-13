import { Component, inject, ChangeDetectorRef, Injectable } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { NgbPagination } from '@ng-bootstrap/ng-bootstrap/pagination';
import { BehaviorSubject, debounceTime, delay, Observable, Subject, switchMap, tap } from 'rxjs';
import { ServerApiService } from '../server-api.service';
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
	//private ref = inject(ChangeDetectorRef);
	//private api = inject(ServerApiService);
	service = inject(SessionsService);
	sessions: Observable<SessionSummary[]>;
	total: Observable<number>;
	now: Date = new Date();
	init: boolean = true;
	Format = Format;

	constructor() {
		this.sessions = this.service.sessions;
		this.total = this.service.total;
		let count = 0;
		console.log('new', this.init);
		this.service.reset();
		this.sessions.subscribe(() => { if(this.init && count++ > 0) this.init = false; });
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
