import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ServerApiServiceToken } from '../server-api.service/server-api.service';
import { ServerLiveServiceToken } from '../server-live.service/server-live.service';

import { Sessions, SessionsService } from './sessions';

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
