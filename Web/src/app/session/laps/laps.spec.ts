import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Laps } from './laps';

describe('Laps', () => {
	let component: Laps;
	let fixture: ComponentFixture<Laps>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [Laps],
			providers: [
				{ provide: ChangeDetectorRef, useValue: {} },
				{ provide: ActivatedRoute, useValue: { snapshot: { paramMap: new Map() } } },
			]
		}).compileComponents();

		fixture = TestBed.createComponent(Laps);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
