import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Sessions } from './sessions/sessions';

@Component({
	selector: 'app-root',
	imports: [RouterOutlet, Sessions],
	templateUrl: './app.html',
	styleUrl: './app.css'
})
export class App {
	protected readonly title = signal('Web');

	constructor() {
	}

	ngOnInit() {
		let check = document.querySelector('input#autorefresh');
		if (check instanceof HTMLInputElement) {
			check.checked = localStorage.getItem(check.id) == "true";
			if (check.checked)
				this.reload(check);
		}
	}

	reload(check: HTMLInputElement) {
		setTimeout(function () {
			if (check.checked)
				window.location.reload();
		}, 10000);
	}

	autorefresh(e: MouseEvent) {
		if (e.target instanceof HTMLInputElement) {
			let check = e.target;
			if (check.checked) {
				localStorage.setItem(check.id, check.checked.toString());
				this.reload(check);
			} else {
				localStorage.removeItem(check.id);
			}
		}
	}
}
