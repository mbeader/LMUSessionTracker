import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Lap as LapModel } from '../../models';
import { Format } from '../../format';
import { ClassBadge } from '../../cars/class-badge/class-badge.component';

@Component({
	selector: 'app-best-laps-lap',
	imports: [RouterLink, ClassBadge],
	templateUrl: './lap.component.html',
	styleUrl: './lap.component.css',
})
export class Lap {
	@Input() lap: LapModel | null = null;
	Format = Format;

	removeBackdrop() {
		document.querySelector('.modal-backdrop')?.remove();
	}
}
