import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BrandBadgeComponent } from './brand-badge.component';

describe('BrandBadgeComponent', () => {
	let component: BrandBadgeComponent;
	let fixture: ComponentFixture<BrandBadgeComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [BrandBadgeComponent]
		})
			.compileComponents();

		fixture = TestBed.createComponent(BrandBadgeComponent);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
