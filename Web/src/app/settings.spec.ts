import { TestBed } from '@angular/core/testing';

import { SettingsService } from './settings.service';

describe('SettingsService', () => {
	let service: SettingsService;
	let localStorage: any;

	beforeEach(() => {
		window.localStorage = localStorage = {
			items: {} as { [key: string]: string },
			getItem(key: string) {
				return this.items[key];
			},
			setItem: vi.fn(),
			removeItem: vi.fn(),
			clear: vi.fn(),
			key: vi.fn(),
			length: 0
		};
		TestBed.configureTestingModule({});
		service = TestBed.inject(SettingsService);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});

	describe('load+get', () => {
		let defaults: any;

		beforeEach(() => {
			defaults = { autotransition: true, fahrenheit: 'true', speed: 'mph' };
		});

		it('without localstorage should return defaults', () => {
			expect(service.get()).toEqual(defaults);
		});

		it('with localstorage missing properties should return default settings', () => {
			localStorage.items = { settings: JSON.stringify({}) };
			service.load();
			expect(service.get()).toEqual(defaults);
		});

		it('with localstorage should return stored default settings', () => {
			localStorage.items = { settings: JSON.stringify(defaults) };
			service.load();
			expect(service.get()).toEqual(defaults);
		});

		it('with localstorage should return stored nondefault settings', () => {
			let settings = { autotransition: false, fahrenheit: 'false', speed: 'km/h' };
			localStorage.items = { settings: JSON.stringify(settings) };
			service.load();
			expect(service.get()).toEqual(settings);
		});

		it('with invalid localstorage should return stored default settings', () => {
			localStorage.items = { settings: JSON.stringify({ autotransition: 'foo', fahrenheit: 'foo', speed: 'foo', invalid: 1 }) };
			service.load();
			expect(service.get()).toEqual(defaults);
		});
	});
});
