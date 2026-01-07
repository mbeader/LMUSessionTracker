import { TestBed } from '@angular/core/testing';

import { ServerApiService } from './server-api.service';

describe('ServerApiService', () => {
	let service: ServerApiService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(ServerApiService);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});
});
