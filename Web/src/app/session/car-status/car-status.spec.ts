import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CarStatus } from './car-status';

describe('CarStatus', () => {
	let component: CarStatus;
	let fixture: ComponentFixture<CarStatus>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [CarStatus]
		})
			.compileComponents();

		fixture = TestBed.createComponent(CarStatus);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
