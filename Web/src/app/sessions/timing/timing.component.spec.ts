import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ServerApiServiceToken } from '../../data/server-api/server-api.service';
import { ServerLiveServiceToken } from '../../data/server-live/server-live.service';

import { TimingComponent } from './timing.component';

describe('TimingComponent', () => {
	let component: TimingComponent;
	let fixture: ComponentFixture<TimingComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [TimingComponent],
			providers: [
				{ provide: ChangeDetectorRef, useValue: {} },
				{ provide: ActivatedRoute, useValue: { snapshot: { paramMap: new Map() } } },
				{ provide: ServerApiServiceToken, useValue: { } },
				{ provide: ServerLiveServiceToken, useValue: { join: vi.fn() } },
			]
		})
			.compileComponents();

		fixture = TestBed.createComponent(TimingComponent);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
