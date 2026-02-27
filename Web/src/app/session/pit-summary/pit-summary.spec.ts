import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PitSummary } from './pit-summary';
import { ChangeDetectorRef } from '@angular/core';

describe('PitSummary', () => {
	let component: PitSummary;
	let fixture: ComponentFixture<PitSummary>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [PitSummary],
			providers: [
				{ provide: ChangeDetectorRef, useValue: {} }
			]
		})
			.compileComponents();

		fixture = TestBed.createComponent(PitSummary);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
