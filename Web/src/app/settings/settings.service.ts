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
	fahrenheit: string;
	speed: string;
}

class SettingsInternal implements Settings {
	autotransition: boolean = true;
	fahrenheit: string = 'true';
	speed: string = 'mph';

	constructor() {
		let us = navigator.language === 'en-US';
		this.fahrenheit = us ? 'true' : 'false';
		this.speed = us ? 'mph' : 'km/h';
	}

	load(settings: Settings) {
		this.autotransition = this.loadBoolean(settings, 'autotransition');
		this.fahrenheit = this.loadBooleanString(settings, 'fahrenheit');
		this.speed = this.loadString(settings, 'speed', ['km/h', 'm/s', 'mph']);
	}

	private loadBoolean(settings: Settings, prop: string): boolean {
		return typeof (settings as any)[prop] === 'boolean' ? (settings as any)[prop] === true : (this as any)[prop];
	}

	private loadBooleanString(settings: Settings, prop: string): 'true' | 'false' {
		let value = (settings as any)[prop];
		return value === 'true' ? 'true' : value === 'false' ? 'false' : (this as any)[prop];
	}

	private loadString(settings: Settings, prop: string, values: string[]): string {
		let value = (settings as any)[prop];
		return values.includes(value) ? value : (this as any)[prop];
	}
}
