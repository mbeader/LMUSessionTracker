import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassBadgeComponent } from './class-badge.component';

describe('ClassBadgeComponent', () => {
	let component: ClassBadgeComponent;
	let fixture: ComponentFixture<ClassBadgeComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [ClassBadgeComponent]
		})
			.compileComponents();

		fixture = TestBed.createComponent(ClassBadgeComponent);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
