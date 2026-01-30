import { Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root',
})
export class Anonymizer {
	private namePool = [
		new FakeName('Adam', 'Adams'),
		new FakeName('Alan', 'Alan'),
		new FakeName('Alex', 'Alexander'),
		new FakeName('Andy', 'Andrews'),
		new FakeName('Austin', 'Austin'),
		new FakeName('Brad', 'Bradley'),
		new FakeName('Carter', 'Carter'),
		new FakeName('Chris', 'Christopher'),
		new FakeName('Christian', 'Christian'),
		new FakeName('Clark', 'Clark'),
		new FakeName('Connor', 'Connors'),
		new FakeName('Dan', 'Daniels'),
		new FakeName('David', 'Davidson'),
		new FakeName('Dean', 'Dean'),
		new FakeName('Don', 'Donaldson'),
		new FakeName('Ed', 'Edwards'),
		new FakeName('Eric', 'Ericsson'),
		new FakeName('George', 'George'),
		new FakeName('Gordon', 'Gordon'),
		new FakeName('Harrison', 'Harrison'),
		new FakeName('Howard', 'Howard'),
		new FakeName('Hugh', 'Hughes'),
		new FakeName('Jack', 'Jackson'),
		new FakeName('Jake', 'Jacobs'),
		new FakeName('Jeff', 'Jeffries'),
		new FakeName('Jim', 'James'),
		new FakeName('Joe', 'Joseph'),
		new FakeName('John', 'Johnson'),
		new FakeName('Jordan', 'Jordan'),
		new FakeName('Keith', 'Keith'),
		new FakeName('Lewis', 'Lewis'),
		new FakeName('Martin', 'Martin'),
		new FakeName('Matt', 'Matthews'),
		new FakeName('Mike', 'Michaels'),
		new FakeName('Mitchell', 'Mitchell'),
		new FakeName('Nick', 'Nichols'),
		new FakeName('Oliver', 'Oliver'),
		new FakeName('Parker', 'Parker'),
		new FakeName('Patrick', 'Patrick'),
		new FakeName('Richard', 'Richard'),
		new FakeName('Riley', 'Riley'),
		new FakeName('Ross', 'Ross'),
		new FakeName('Ryan', 'Ryan'),
		new FakeName('Sam', 'Samuels'),
		new FakeName('Scott', 'Scott'),
		new FakeName('Spencer', 'Spencer'),
		new FakeName('Steve', 'Stevens'),
		new FakeName('Stewart', 'Stewart'),
		new FakeName('Taylor', 'Taylor'),
		new FakeName('Todd', 'Todd'),
		new FakeName('Tom', 'Thomas'),
		new FakeName('Tony', 'Anthony'),
		new FakeName('Tyler', 'Tyler'),
		new FakeName('Will', 'Williams'),
		new FakeName('Wilson', 'Wilson'),
	];
	private mapping = new Map<string, string>();
	private reverseMapping = new Map<string, string>();

	driver(name: string) {
		let fake = this.mapping.get(name);
		if (!fake) {
			let firstIndex = this.random();
			let lastIndex;
			do {
				lastIndex = this.random();
				fake = `${this.namePool[firstIndex].first} ${this.namePool[lastIndex].last}`;
			} while (firstIndex == lastIndex || this.reverseMapping.get(fake));
			this.mapping.set(name, fake);
			this.reverseMapping.set(fake, name);
		}
		return fake;
	}

	team(number: string, carClass: string) {
		return `${carClass} #${number}`;
	}

	private random() {
		return Math.floor(Math.random() * this.namePool.length);
	}
}

class FakeName {
	public first: string;
	public last: string;

	constructor(first: string, last: string) {
		this.first = first;
		this.last = last;
	}
}
