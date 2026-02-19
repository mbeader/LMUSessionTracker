import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ServerLiveService } from '../../server-live.service';

import { Timing } from './timing';

describe('Timing', () => {
	let component: Timing;
	let fixture: ComponentFixture<Timing>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [Timing],
			providers: [
				{ provide: ChangeDetectorRef, useValue: {} },
				{ provide: ActivatedRoute, useValue: { snapshot: { paramMap: new Map() } } },
				{ provide: ServerLiveService, useValue: { join: vi.fn() } },
			]
		})
			.compileComponents();

		fixture = TestBed.createComponent(Timing);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
