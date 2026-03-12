import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ServerApiServiceToken } from '../../data/server-api/server-api.service';
import { ServerLiveServiceToken } from '../../data/server-live/server-live.service';

import { Chat } from './chat.component';

describe('Chat', () => {
	let component: Chat;
	let fixture: ComponentFixture<Chat>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [Chat],
			providers: [
				{ provide: ChangeDetectorRef, useValue: {} },
				{ provide: ActivatedRoute, useValue: { snapshot: { paramMap: new Map() } } },
				{ provide: ServerApiServiceToken, useValue: {} },
				{ provide: ServerLiveServiceToken, useValue: {} },
			]
		})
			.compileComponents();

		fixture = TestBed.createComponent(Chat);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});

	describe('generatePossibleSenderNames', () => {
		test.each([
			{ test: 'no spaces', name: 'foobar', ex: ['foobar'] },
			{ test: 'single space normal capitalization', name: 'Foo Bar', ex: ['F Bar'] },
			{ test: 'single space all lowercase', name: 'foo bar', ex: ['f bar'] },
			{ test: 'single space all uppercase', name: 'FOO BAR', ex: ['F BAR'] },
			{ test: 'two spaces normal capitalization', name: 'Foo Bar Baz', ex: ['F Bar Baz', 'F Baz', 'F B Baz'] },
			{ test: 'single space over 15 characters', name: 'Foo BarBazBarBazBarBaz', ex: ['F BarBazBarBazBarBaz', 'F BarBazBarBazB'] },
			{ test: 'three spaces', name: 'Foo Bar X Baz', ex: ['F Bar X Baz', 'F X Baz', 'F B X Baz', 'F Baz'] },
			{ test: 'three spaces over 15 characters', name: 'Foo Bar X BarBazBarBaz', ex: ['F Bar X BarBazBarBaz', 'F Bar X BarBazB', 'F X BarBazBarBaz', 'F X BarBazBarBa', 'F B X BarBazBarBaz', 'F B X BarBazBar', 'F BarBazBarBaz'] },
		])('$test', ({ test, name, ex }) => {
			expect(Chat.generatePossibleSenderNames(name)).toEqual(ex);
		});
	});
});
