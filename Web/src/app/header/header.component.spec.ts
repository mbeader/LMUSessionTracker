import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, UrlSegment } from '@angular/router';

import { HeaderComponent } from './header.component';

describe('HeaderComponent', () => {
	let component: HeaderComponent;
	let fixture: ComponentFixture<HeaderComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [HeaderComponent],
			providers: [
				{ provide: ChangeDetectorRef, useValue: {} },
				{ provide: ActivatedRoute, useValue: { snapshot: { paramMap: new Map() } } },
			]
		})
			.compileComponents();

		fixture = TestBed.createComponent(HeaderComponent);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});

	describe('findCurrentNavLink', () => {
		test('root should be home', () => {
			expect(HeaderComponent.findCurrentNavLink([])).toEqual({ navLink: 'home', navSubLink: '' });
		});

		test('session should be session-live', () => {
			expect(HeaderComponent.findCurrentNavLink([
				new UrlSegment('Session', {}),
				new UrlSegment('id', {})
			])).toEqual({ navLink: 'session', navSubLink: 'live' });
		});

		test('session-laps should be session-live', () => {
			expect(HeaderComponent.findCurrentNavLink([
				new UrlSegment('Session', {}),
				new UrlSegment('id', {}),
				new UrlSegment('Laps', {}),
				new UrlSegment('id', {}),
			])).toEqual({ navLink: 'session', navSubLink: 'live' });
		});

		test('anything else should be empty', () => {
			expect(HeaderComponent.findCurrentNavLink([new UrlSegment('foo', {})])).toEqual({ navLink: '', navSubLink: '' });
		});
	});
});
