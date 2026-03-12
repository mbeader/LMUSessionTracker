import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CarStatusComponent } from './car-status.component';

describe('CarStatusComponent', () => {
	let component: CarStatusComponent;
	let fixture: ComponentFixture<CarStatusComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [CarStatusComponent]
		})
			.compileComponents();

		fixture = TestBed.createComponent(CarStatusComponent);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
