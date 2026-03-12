import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ServerApiServiceToken } from '../../data/server-api/server-api.service';
import { ServerLiveServiceToken } from '../../data/server-live/server-live.service';

import { SessionComponent } from './session.component';

describe('SessionComponent', () => {
	let component: SessionComponent;
	let fixture: ComponentFixture<SessionComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [SessionComponent],
			providers: [
				{ provide: ChangeDetectorRef, useValue: {} },
				{ provide: ActivatedRoute, useValue: { snapshot: { paramMap: new Map() } } },
				{ provide: ServerApiServiceToken, useValue: { } },
				{ provide: ServerLiveServiceToken, useValue: { join: vi.fn() } },
			]
		}).compileComponents();

		fixture = TestBed.createComponent(SessionComponent);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
