import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ChangeDetectorRef } from '@angular/core';

import { PitSummaryComponent } from './pit-summary.component';

describe('PitSummaryComponent', () => {
	let component: PitSummaryComponent;
	let fixture: ComponentFixture<PitSummaryComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [PitSummaryComponent],
			providers: [
				{ provide: ChangeDetectorRef, useValue: {} }
			]
		})
			.compileComponents();

		fixture = TestBed.createComponent(PitSummaryComponent);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
