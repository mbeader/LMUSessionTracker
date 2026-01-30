import { Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root',
})
export class AlertService {
	showAlert(error: any) {
		let title = '', message = '';
		if (error.name == 'HttpErrorResponse') {
			title = 'Request failed:';
			if (error.status == 0)
				message = 'Could not connect to server';
			else
				message = error.status + ' ' + error.statusText;
		} else {
			title = 'Something happened';
			message = 'Something happened';
		}
		document.querySelector('main')?.prepend(this.buildAlert(title, message));
	}

	private buildAlert(titleText: string, messageText: string) {
		let alert = document.createElement('div');
		alert.className = 'alert alert-danger alert-dismissible fade show';
		alert.role = 'alert';
		let title = document.createElement('strong');
		title.textContent = titleText;
		let message = document.createTextNode(' ' + messageText);
		let button = document.createElement('button');
		button.type = 'button';
		button.className = 'btn-close';
		button.ariaLabel = 'Close';
		button.setAttribute('data-bs-dismiss', 'alert');
		alert.appendChild(title);
		alert.appendChild(message);
		alert.appendChild(button);
		return alert;
	}
}
