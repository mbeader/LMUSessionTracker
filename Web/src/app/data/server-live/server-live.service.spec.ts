import { TestBed } from '@angular/core/testing';

import { HttpServerLiveService, ServerLiveService, ServerLiveServiceToken } from './server-live.service';

describe('ServerLiveService', () => {
	let service: ServerLiveService;

	beforeEach(() => {
		TestBed.configureTestingModule({
			providers: [
				{ provide: ServerLiveServiceToken, useValue: HttpServerLiveService },
			]
		});
		service = TestBed.inject(ServerLiveServiceToken);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});
});
