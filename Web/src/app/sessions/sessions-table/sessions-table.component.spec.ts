import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SessionsTableComponent } from './sessions-table.component';

describe('SessionsTableComponent', () => {
	let component: SessionsTableComponent;
	let fixture: ComponentFixture<SessionsTableComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [SessionsTableComponent]
		})
			.compileComponents();

		fixture = TestBed.createComponent(SessionsTableComponent);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
