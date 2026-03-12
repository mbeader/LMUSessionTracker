import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { ServerApiServiceToken } from '../../data/server-api/server-api.service';
import { ServerLiveServiceToken } from '../../data/server-live/server-live.service';

import { TrackMapComponent } from './track-map.component';

describe('TrackMapComponent', () => {
	let component: TrackMapComponent;
	let fixture: ComponentFixture<TrackMapComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [TrackMapComponent],
			providers: [
				{ provide: ActivatedRoute, useValue: { snapshot: { paramMap: new Map() } } },
				{ provide: ServerApiServiceToken, useValue: { } },
				{ provide: ServerLiveServiceToken, useValue: { } },
			]
		})
			.compileComponents();

		fixture = TestBed.createComponent(TrackMapComponent);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
