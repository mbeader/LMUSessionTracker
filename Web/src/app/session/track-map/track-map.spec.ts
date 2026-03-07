import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { ServerApiServiceToken } from '../../server-api.service/server-api.service';
import { ServerLiveServiceToken } from '../../server-live.service/server-live.service';

import { TrackMap } from './track-map';

describe('TrackMap', () => {
	let component: TrackMap;
	let fixture: ComponentFixture<TrackMap>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [TrackMap],
			providers: [
				{ provide: ActivatedRoute, useValue: { snapshot: { paramMap: new Map() } } },
				{ provide: ServerApiServiceToken, useValue: { } },
				{ provide: ServerLiveServiceToken, useValue: { } },
			]
		})
			.compileComponents();

		fixture = TestBed.createComponent(TrackMap);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
