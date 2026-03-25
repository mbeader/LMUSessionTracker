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

	describe('lapProgress', () => {
		test.each([
			{ progress: 0, total: 1000, ex: '0' },
			{ progress: 500, total: 1000, ex: '5' },
			{ progress: 1000, total: 1000, ex: '9' },
			{ progress: 2000, total: 1000, ex: '9' },
			{ progress: -0, total: 1000, ex: '0' },
			{ progress: -500, total: 1000, ex: '-5' },
			{ progress: -1000, total: 1000, ex: '-9' },
			{ progress: -2000, total: 1000, ex: '-9' },
			{ progress: null, total: 1000, ex: '0' },
			{ progress: 0, total: null, ex: '0' },
			{ progress: null, total: null, ex: '0' },
		])('$progress of $total should return $ex', ({ progress, total, ex }) => {
			expect(Format.lapProgress(progress, total)).toBe(ex);
		});
	});
});
