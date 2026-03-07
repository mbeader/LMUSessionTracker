import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ServerApiServiceToken } from '../server-api.service/server-api.service';
import { ServerLiveServiceToken } from '../server-live.service/server-live.service';

import { About } from './about';

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
