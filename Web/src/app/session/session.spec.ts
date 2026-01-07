import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Session } from './session';

describe('Session', () => {
	let component: Session;
	let fixture: ComponentFixture<Session>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [Session],
			providers: [
				{ provide: ChangeDetectorRef, useValue: {} },
				{ provide: ActivatedRoute, useValue: { snapshot: { paramMap: new Map() } } },
			]
		}).compileComponents();

		fixture = TestBed.createComponent(Session);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
