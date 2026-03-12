import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BrandBadge } from './brand-badge.component';

describe('BrandBadge', () => {
	let component: BrandBadge;
	let fixture: ComponentFixture<BrandBadge>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [BrandBadge]
		})
			.compileComponents();

		fixture = TestBed.createComponent(BrandBadge);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
