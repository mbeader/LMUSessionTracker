import { TestBed } from '@angular/core/testing';

import { ServerLiveService } from './server-live.service';

describe('ServerLiveService', () => {
	let service: ServerLiveService;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(ServerLiveService);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});
});
