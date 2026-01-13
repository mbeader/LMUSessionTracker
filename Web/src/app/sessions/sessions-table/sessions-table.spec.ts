import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SessionsTable } from './sessions-table';

describe('SessionsTable', () => {
	let component: SessionsTable;
	let fixture: ComponentFixture<SessionsTable>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [SessionsTable]
		})
			.compileComponents();

		fixture = TestBed.createComponent(SessionsTable);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
