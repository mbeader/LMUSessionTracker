import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Lap, Pit } from '../../models';
import { Vehicle } from '../../tracking';
import { Format } from '../../format';
import { ClassBadgeComponent } from '../../cars/class-badge/class-badge.component';
import { BrandBadgeComponent } from '../../cars/brand-badge/brand-badge.component';
import { TireBadgeComponent } from '../../cars/tire-badge/tire-badge.component';

@Component({
	selector: 'app-best-laps-lap',
	imports: [RouterLink, ClassBadgeComponent, BrandBadgeComponent, TireBadgeComponent],
	templateUrl: './lap.component.html',
	styleUrl: './lap.component.css',
})
export class LapComponent {
	@Input() lap?: Lap;
	@Input() pit?: Pit;
	@Input() vehicle?: Vehicle;
	Format = Format;

	removeBackdrop() {
		document.querySelector('.modal-backdrop')?.remove();
	}
}
