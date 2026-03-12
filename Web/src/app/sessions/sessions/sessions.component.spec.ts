import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ServerApiServiceToken } from '../../data/server-api/server-api.service';
import { ServerLiveServiceToken } from '../../data/server-live/server-live.service';

import { SessionsComponent, SessionsService } from './sessions.component';

describe('SessionsComponent', () => {
	let component: SessionsComponent;
	let fixture: ComponentFixture<SessionsComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [SessionsComponent],
			providers: [
				{ provide: ServerApiServiceToken, useValue: { getLiveSessions: vi.fn().mockReturnValueOnce({ subscribe: vi.fn() }) } },
				{ provide: ServerLiveServiceToken, useValue: { join: vi.fn() } },
				{ provide: SessionsService, useValue: { sessions: { subscribe: vi.fn() } } }
			]
		}).compileComponents();

		fixture = TestBed.createComponent(SessionsComponent);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
