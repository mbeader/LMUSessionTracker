import { Component } from '@angular/core';
import packageJson from '../../../package.json';

@Component({
	selector: 'app-about',
	imports: [],
	templateUrl: './about.html',
	styleUrl: './about.css',
})
export class About {
	version: string = packageJson.version;
}
