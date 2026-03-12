import { TestBed } from '@angular/core/testing';

import { HttpServerApiService, ServerApiService, ServerApiServiceToken } from './server-api.service';

describe('ServerApiService', () => {
	let service: ServerApiService;

	beforeEach(() => {
		TestBed.configureTestingModule({
			providers: [
				{ provide: ServerApiServiceToken, useValue: HttpServerApiService },
			]
		});
		service = TestBed.inject(ServerApiServiceToken);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});
});
