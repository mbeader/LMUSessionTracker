import { TestBed } from '@angular/core/testing';

import { Anonymizer } from './anonymizer.service';

describe('Anonymizer', () => {
	let service: Anonymizer;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(Anonymizer);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});
});
