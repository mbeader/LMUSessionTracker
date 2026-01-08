import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Standings } from './standings';

describe('Standings', () => {
	let component: Standings;
	let fixture: ComponentFixture<Standings>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [Standings]
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
