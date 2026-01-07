import { TestBed } from '@angular/core/testing';

import { Format } from './format';

describe('Format', () => {
	beforeEach(() => {
		TestBed.configureTestingModule({});
	});

	describe('diff', () => {
		test('lead lap should return time', () => {
			expect(Format.diff(0, 1.2345)).toBe('1.235');
		});

		test('lap down should return laps', () => {
			expect(Format.diff(1, 1.2345)).toBe('1 L');
		});
	});
});
