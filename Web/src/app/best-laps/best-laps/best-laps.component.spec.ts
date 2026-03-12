import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ServerApiServiceToken } from '../../data/server-api/server-api.service';
import { ServerLiveServiceToken } from '../../data/server-live/server-live.service';

import { BestLapsComponent } from './best-laps.component';

describe('BestLapsComponent', () => {
	let component: BestLapsComponent;
	let fixture: ComponentFixture<BestLapsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [BestLapsComponent],
			providers: [
				{ provide: ChangeDetectorRef, useValue: {} },
				{ provide: ActivatedRoute, useValue: { snapshot: { paramMap: new Map() }, queryParamMap: { subscribe: vi.fn() } } },
				{ provide: ServerApiServiceToken, useValue: { getTracks: vi.fn().mockReturnValue({ then: vi.fn() }) } },
				{ provide: ServerLiveServiceToken, useValue: { } },
			]
		})
			.compileComponents();

		fixture = TestBed.createComponent(BestLapsComponent);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
