import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ServerApiServiceToken } from '../data/server-api/server-api.service';
import { ServerLiveServiceToken } from '../data/server-live/server-live.service';

import { About } from './about.component';

describe('About', () => {
	let component: About;
	let fixture: ComponentFixture<About>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [About],
			providers: [
				{ provide: ServerApiServiceToken, useValue: { getAbout: vi.fn().mockReturnValueOnce({ subscribe: vi.fn() }) } },
			]
		})
			.compileComponents();

		fixture = TestBed.createComponent(About);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
