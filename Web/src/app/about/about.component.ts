import { Component, inject } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { BehaviorSubject, Observable } from 'rxjs';
import { ServerApiService, ServerApiServiceToken } from '../data/server-api/server-api.service';
import { AboutOptions } from '../view-models';
import { environment } from '../../environments/environment';
import packageJson from '../../../package.json';

@Component({
	selector: 'app-about',
	imports: [AsyncPipe],
	templateUrl: './about.component.html',
	styleUrl: './about.component.css',
})
export class AboutComponent {
	private _aboutOptions = new BehaviorSubject<AboutOptions>(new AboutOptions());
	private api = inject(ServerApiServiceToken);
	static: boolean = environment.static;
	version: string = packageJson.version;
	aboutOptions: Observable<AboutOptions>;
	url: string;
	AboutOptions = AboutOptions;

	constructor() {
		this.aboutOptions = this._aboutOptions.asObservable();
		this.api.getAbout().subscribe((result) => {
			this._aboutOptions.next(result ?? new AboutOptions());
		});
		this.url = window.location.origin + '/';
	}
}
