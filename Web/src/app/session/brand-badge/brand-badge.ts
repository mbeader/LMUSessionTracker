import { Component, Input } from '@angular/core';

@Component({
	selector: 'app-session-brand-badge',
	imports: [],
	templateUrl: './brand-badge.html',
	styleUrl: './brand-badge.css',
})
export class BrandBadge {
	@Input() brand?: string;
	@Input() center: boolean = true;

	getId() {
		if (this.brand)
			return `brand-${this.brand.replace(' ', '-').toLowerCase()}`
		return 'brand-unknown';
	}
}
