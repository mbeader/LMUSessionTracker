import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassBadge } from './class-badge.component';

describe('ClassBadge', () => {
	let component: ClassBadge;
	let fixture: ComponentFixture<ClassBadge>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [ClassBadge]
		})
			.compileComponents();

		fixture = TestBed.createComponent(ClassBadge);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
