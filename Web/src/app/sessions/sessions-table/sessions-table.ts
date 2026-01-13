import { Component, Input } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Observable } from 'rxjs';
import { SessionSummary } from '../../tracking';
import { Format } from '../../format';

@Component({
	selector: 'app-sessions-sessions-table',
	imports: [RouterLink, AsyncPipe],
	templateUrl: './sessions-table.html',
	styleUrl: './sessions-table.css',
})
export class SessionsTable {
	@Input() sessions: Observable<SessionSummary[]> = new Observable();
	@Input() init: boolean = true;
	now: Date = new Date();
	Format = Format;
}
