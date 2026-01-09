import { Component, Input } from '@angular/core';
import { RouterLink } from '@angular/router';
import { SessionViewModel } from '../../view-models';
import { Car, CarKey } from '../../tracking';
import { Format } from '../../format';

@Component({
	selector: 'app-session-standings',
	imports: [RouterLink],
	templateUrl: './standings.html',
	styleUrl: './standings.css',
})
export class Standings {
	@Input() session: SessionViewModel | null = null;
	entries: Map<string, Car> = new Map();
	positionInClass: Map<string, number> = new Map();
	Format = Format;
	CarKey = CarKey;

	ngOnChanges() {
		this.entries.clear();
		if (this.session && this.session.entries) {
			for (let car in this.session.entries) {
				this.entries.set(car ?? '', this.session.entries[car]);
			}
		}
		this.positionInClass.clear();
		if (this.session && this.session.positionInClass) {
			for (let car in this.session.positionInClass) {
				this.positionInClass.set(car ?? '', this.session.positionInClass[car]);
			}
		}
	}
}
