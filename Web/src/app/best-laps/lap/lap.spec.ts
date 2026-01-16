import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Lap } from './lap';

describe('Lap', () => {
	let component: Lap;
	let fixture: ComponentFixture<Lap>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [Lap]
		})
			.compileComponents();

		fixture = TestBed.createComponent(Lap);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
