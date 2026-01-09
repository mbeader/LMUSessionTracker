import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { History } from './history';

describe('History', () => {
	let component: History;
	let fixture: ComponentFixture<History>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [History],
			providers: [
				{ provide: ChangeDetectorRef, useValue: {} },
				{ provide: ActivatedRoute, useValue: { snapshot: { paramMap: new Map() } } },
			]
		})
			.compileComponents();

		fixture = TestBed.createComponent(History);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
