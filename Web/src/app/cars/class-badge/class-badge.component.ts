import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-cars-class-badge',
  imports: [],
  templateUrl: './class-badge.component.html',
  styleUrl: './class-badge.component.css',
})
export class ClassBadgeComponent {
	@Input() carClass: string | null = null;
	@Input() compact: boolean = false;
}
