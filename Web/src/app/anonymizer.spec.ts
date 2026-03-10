import { TestBed } from '@angular/core/testing';

import { Anonymizer } from './anonymizer.service';

describe('Anonymizer', () => {
	let service: Anonymizer;

	beforeEach(() => {
		TestBed.configureTestingModule({});
		service = TestBed.inject(Anonymizer);
	});

	it('should be created', () => {
		expect(service).toBeTruthy();
	});

	//it('generate', () => {
	//	let names = [
	//	];
	//	let conversions = [];
	//	let shortConversions = [];
	//	for (let name of names) {
	//		let anonymized = service.driver(name);
	//		conversions.push(`('${name}', '${anonymized}'),`);
	//		let shortNames = [];
	//		let space = name.indexOf(' ');
	//		while (space > 0) {
	//			shortNames.push(`${name[0]} ${name.substring(space + 1)}`);
	//			space = name.indexOf(' ', space + 1);
	//		}

	//		let anonymizedSplit = anonymized.split(' ');
	//		for (let shortName of shortNames) {
	//			shortConversions.push(`('${name}', '${shortName}', '${anonymizedSplit[0][0]} ${anonymizedSplit[1]}'),`);
	//		}
	//	}
	//	console.log(conversions.join('\r\n'));
	//	console.log(shortConversions.join('\r\n'));
	//});
});
