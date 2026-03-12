import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Lap } from '../../models';
import { Format } from '../../format';
import { ClassBadgeComponent } from '../../cars/class-badge/class-badge.component';

@Component({
	selector: 'app-best-laps-lap',
	imports: [RouterLink, ClassBadgeComponent],
	templateUrl: './lap.component.html',
	styleUrl: './lap.component.css',
})
export class LapComponent {
	@Input() lap: Lap | null = null;
	Format = Format;

	removeBackdrop() {
		document.querySelector('.modal-backdrop')?.remove();
	}
}
