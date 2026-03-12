import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LapComponent } from './lap.component';

describe('LapComponent', () => {
	let component: LapComponent;
	let fixture: ComponentFixture<LapComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [LapComponent]
		})
			.compileComponents();

		fixture = TestBed.createComponent(LapComponent);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
