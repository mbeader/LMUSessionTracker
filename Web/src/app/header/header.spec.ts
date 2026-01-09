import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, UrlSegment } from '@angular/router';

import { Header } from './header';

describe('Header', () => {
	let component: Header;
	let fixture: ComponentFixture<Header>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [Header],
			providers: [
				{ provide: ChangeDetectorRef, useValue: {} },
				{ provide: ActivatedRoute, useValue: { snapshot: { paramMap: new Map() } } },
			]
		})
			.compileComponents();

		fixture = TestBed.createComponent(Header);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});

	describe('findCurrentNavLink', () => {
		test('root should be home', () => {
			expect(Header.findCurrentNavLink([])).toEqual({ navLink: 'home', navSubLink: '' });
		});

		test('session should be session-live', () => {
			expect(Header.findCurrentNavLink([
				new UrlSegment('Session', {}),
				new UrlSegment('id', {})
			])).toEqual({ navLink: 'session', navSubLink: 'live' });
		});

		test('session-laps should be session-live', () => {
			expect(Header.findCurrentNavLink([
				new UrlSegment('Session', {}),
				new UrlSegment('id', {}),
				new UrlSegment('Laps', {}),
				new UrlSegment('id', {}),
			])).toEqual({ navLink: 'session', navSubLink: 'live' });
		});

		test('session-history should be session-history', () => {
			expect(Header.findCurrentNavLink([
				new UrlSegment('Session', {}),
				new UrlSegment('id', {}),
				new UrlSegment('History', {})
			])).toEqual({ navLink: 'session', navSubLink: 'history' });
		});

		test('session-history-laps should be session-history', () => {
			expect(Header.findCurrentNavLink([
				new UrlSegment('Session', {}),
				new UrlSegment('id', {}),
				new UrlSegment('History', {}),
				new UrlSegment('Laps', {}),
				new UrlSegment('id', {}),
			])).toEqual({ navLink: 'session', navSubLink: 'history' });
		});

		test('anything else should be empty', () => {
		});
			expect(Header.findCurrentNavLink([new UrlSegment('foo', {})])).toEqual({ navLink: '', navSubLink: '' });
	});
});
