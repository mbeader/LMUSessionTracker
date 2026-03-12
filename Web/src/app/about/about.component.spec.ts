import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ServerApiServiceToken } from '../data/server-api/server-api.service';
import { ServerLiveServiceToken } from '../data/server-live/server-live.service';

import { AboutComponent } from './about.component';

describe('AboutComponent', () => {
	let component: AboutComponent;
	let fixture: ComponentFixture<AboutComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [AboutComponent],
			providers: [
				{ provide: ServerApiServiceToken, useValue: { getAbout: vi.fn().mockReturnValueOnce({ subscribe: vi.fn() }) } },
			]
		})
			.compileComponents();

		fixture = TestBed.createComponent(AboutComponent);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
