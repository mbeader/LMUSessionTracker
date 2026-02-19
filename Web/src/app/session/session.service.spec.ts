import { TestBed } from '@angular/core/testing';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ServerLiveService } from '../server-live.service';

import { SessionService } from './session.service';

describe('SessionService', () => {
	let service: SessionService;

	beforeEach(() => {
		TestBed.configureTestingModule({
			providers: [
				{ provide: ChangeDetectorRef, useValue: {} },
				{ provide: ActivatedRoute, useValue: { snapshot: { paramMap: new Map() } } },
				{ provide: ServerLiveService, useValue: { join: vi.fn() } },
				SessionService
			]
		});
		service = TestBed.inject(SessionService);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});
});
