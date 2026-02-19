import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-session-class-badge',
  imports: [],
  templateUrl: './class-badge.html',
  styleUrl: './class-badge.css',
})
export class ClassBadge {
	@Input() carClass: string | null = null;
	@Input() compact: boolean = false;
}
