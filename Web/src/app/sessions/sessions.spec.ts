import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Sessions } from './sessions';
import { ServerLiveService } from '../server-live.service';
import { ServerApiService } from '../server-api.service';

describe('Sessions', () => {
	let component: Sessions;
	let fixture: ComponentFixture<Sessions>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [Sessions],
			providers: [
				{ provide: ServerApiService, useValue: { getLiveSessions: vi.fn().mockReturnValueOnce({ subscribe: vi.fn() }) } },
				{ provide: ServerLiveService, useValue: { join: vi.fn() } },
			]
		}).compileComponents();

		fixture = TestBed.createComponent(Sessions);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
