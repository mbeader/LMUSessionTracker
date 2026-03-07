import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ServerApiServiceToken } from '../../server-api.service/server-api.service';
import { ServerLiveServiceToken } from '../../server-live.service/server-live.service';

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
				{ provide: ServerApiServiceToken, useValue: { } },
				{ provide: ServerLiveServiceToken, useValue: { } },
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
