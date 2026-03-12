import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ServerApiServiceToken } from '../../data/server-api/server-api.service';
import { ServerLiveServiceToken } from '../../data/server-live/server-live.service';

import { Sessions, SessionsService } from './sessions.component';

describe('Sessions', () => {
	let component: Sessions;
	let fixture: ComponentFixture<Sessions>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [Sessions],
			providers: [
				{ provide: ServerApiServiceToken, useValue: { getLiveSessions: vi.fn().mockReturnValueOnce({ subscribe: vi.fn() }) } },
				{ provide: ServerLiveServiceToken, useValue: { join: vi.fn() } },
				{ provide: SessionsService, useValue: { sessions: { subscribe: vi.fn() } } }
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
