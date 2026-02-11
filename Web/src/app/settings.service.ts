import { Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root',
})
export class SettingsService {
	private settings: SettingsInternal = new SettingsInternal();

	constructor() {
		this.load();
	}

	load() {
		let json = window.localStorage.getItem('settings');
		let settings;
		if (json) {
			try {
				settings = JSON.parse(json);
			} catch {
			}
		}
		this.set(settings);
	}

	get() {
		return Object.assign(new Object(), this.settings) as Settings;
	}

	set(settings: any) {
		if (settings)
			this.settings.load(settings);
		window.localStorage.setItem('settings', JSON.stringify(this.settings));
	}
}

interface Settings {
	autotransition: boolean;
}

class SettingsInternal implements Settings {
	autotransition: boolean = true;

	load(settings: Settings) {
		this.autotransition = this.loadBoolean(settings, 'autotransition');
	}

	private loadBoolean(settings: Settings, prop: string): boolean {
		return typeof (settings as any)[prop] === 'boolean' ? (settings as any)[prop] === true : (this as any)[prop];
	}
}
