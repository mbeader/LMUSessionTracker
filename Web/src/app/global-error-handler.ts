import { ErrorHandler, inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { AlertService } from './alert.service';

@Injectable({
	providedIn: 'root',
})
export class GlobalErrorHandler implements ErrorHandler {
	private readonly router = inject(Router);
	private readonly alertService = inject(AlertService);

	handleError(error: any) {
		const url = this.router.url;
		this.alertService.showAlert(error);
		console.error(url, GlobalErrorHandler.name, { error });
	}
}
