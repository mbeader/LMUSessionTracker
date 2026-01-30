import { Component, inject } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { BehaviorSubject, Observable } from 'rxjs';
import { ServerApiService } from '../server-api.service';
import { AboutOptions } from '../view-models';
import packageJson from '../../../package.json';

@Component({
	selector: 'app-about',
	imports: [AsyncPipe],
	templateUrl: './about.html',
	styleUrl: './about.css',
})
export class About {
	private _aboutOptions = new BehaviorSubject<AboutOptions>(new AboutOptions());
	private api = inject(ServerApiService);
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
