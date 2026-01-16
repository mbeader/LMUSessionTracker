import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Lap as LapModel } from '../../models';
import { Format } from '../../format';
import { ClassBadge } from '../../session/class-badge/class-badge';

@Component({
	selector: 'app-best-laps-lap',
	imports: [RouterLink, ClassBadge],
	templateUrl: './lap.html',
	styleUrl: './lap.css',
})
export class Lap {
	@Input() lap: LapModel | null = null;
	Format = Format;
}
