import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ServerApiServiceToken } from '../../data/server-api/server-api.service';
import { ServerLiveServiceToken } from '../../data/server-live/server-live.service';

import { Standings } from './standings.component';

describe('Standings', () => {
	let component: Standings;
	let fixture: ComponentFixture<Standings>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [Standings],
			providers: [
				{ provide: ServerApiServiceToken, useValue: { } },
				{ provide: ServerLiveServiceToken, useValue: { } },
			]
		})
			.compileComponents();

		fixture = TestBed.createComponent(Standings);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
