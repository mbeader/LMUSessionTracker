import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SettingsService } from '../settings.service';

@Component({
	selector: 'app-settings',
	imports: [FormsModule],
	templateUrl: './settings.html',
	styleUrl: './settings.css',
})
export class Settings {
	private settingsService = inject(SettingsService);
	settings: any;

	constructor() {
		this.settings = this.settingsService.get();
	}

	onChange() {
		let form = document.querySelector<HTMLFormElement>('form#settings');
		if (form)
			this.settingsService.set(this.readForm(form));
	}

	private readForm(form: HTMLFormElement) {
		let settings: { [key: string]: any } = new Object();
		for (let check of form.querySelectorAll<HTMLInputElement>('input[type=checkbox]'))
			settings[check.id] = check.checked;
		return settings;
	}
}
