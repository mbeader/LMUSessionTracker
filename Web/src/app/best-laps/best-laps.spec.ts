import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { BestLaps } from './best-laps';

describe('BestLaps', () => {
	let component: BestLaps;
	let fixture: ComponentFixture<BestLaps>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [BestLaps],
			providers: [
				{ provide: ChangeDetectorRef, useValue: {} },
				{ provide: ActivatedRoute, useValue: { snapshot: { paramMap: new Map() }, queryParamMap: { subscribe: vi.fn() } } }
			]
		})
			.compileComponents();

		fixture = TestBed.createComponent(BestLaps);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
