import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ServerApiServiceToken } from '../../data/server-api/server-api.service';
import { ServerLiveServiceToken } from '../../data/server-live/server-live.service';

import { StandingsComponent } from './standings.component';

describe('StandingsComponent', () => {
	let component: StandingsComponent;
	let fixture: ComponentFixture<StandingsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [StandingsComponent],
			providers: [
				{ provide: ServerApiServiceToken, useValue: { } },
				{ provide: ServerLiveServiceToken, useValue: { } },
			]
		})
			.compileComponents();

		fixture = TestBed.createComponent(StandingsComponent);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
