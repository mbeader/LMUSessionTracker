import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Results } from './results';

describe('Results', () => {
	let component: Results;
	let fixture: ComponentFixture<Results>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [Results],
			providers: [
				{ provide: ChangeDetectorRef, useValue: {} },
				{ provide: ActivatedRoute, useValue: { snapshot: { paramMap: new Map() } } },
			]
		})
			.compileComponents();

		fixture = TestBed.createComponent(Results);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
