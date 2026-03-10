import { Injectable } from '@angular/core';

@Injectable({
	providedIn: 'root',
})
export class Anonymizer {
	private namePool = [
		new FakeName('Adam', 'Adams'),
		new FakeName('Alan', 'Alan'),
		new FakeName('Alex', 'Alexander'),
		new FakeName('Allen', 'Allen'),
		new FakeName('Andy', 'Andrews'),
		new FakeName('Austin', 'Austin'),
		new FakeName('Ben', 'Benjamin'),
		new FakeName('Brad', 'Bradley'),
		new FakeName('Brian', 'Bryant'),
		new FakeName('Carl', 'Carlson'),
		new FakeName('Carter', 'Carter'),
		new FakeName('Casey', 'Casey'),
		new FakeName('Charlie', 'Charles'),
		new FakeName('Chase', 'Chase'),
		new FakeName('Chris', 'Christopher'),
		new FakeName('Christian', 'Christian'),
		new FakeName('Clark', 'Clark'),
		new FakeName('Clint', 'Clinton'),
		new FakeName('Cole', 'Cole'),
		new FakeName('Connor', 'Connors'),
		new FakeName('Corey', 'Corey'),
		new FakeName('Curtis', 'Curtis'),
		new FakeName('Dan', 'Daniels'),
		new FakeName('David', 'Davidson'),
		new FakeName('Dean', 'Dean'),
		new FakeName('Dennis', 'Dennison'),
		new FakeName('Dick', 'Dick'),
		new FakeName('Don', 'Donaldson'),
		new FakeName('Dylan', 'Dillon'),
		new FakeName('Earl', 'Earl'),
		new FakeName('Ed', 'Edwards'),
		new FakeName('Elliott', 'Elliott'),
		new FakeName('Eric', 'Ericsson'),
		new FakeName('Ernie', 'Earnest'),
		new FakeName('Frank', 'Frank'),
		new FakeName('George', 'George'),
		new FakeName('Glen', 'Glenn'),
		new FakeName('Gordon', 'Gordon'),
		new FakeName('Greg', 'Gregory'),
		new FakeName('Hamilton', 'Hamilton'),
		new FakeName('Harold', 'Harold'),
		new FakeName('Harrison', 'Harrison'),
		new FakeName('Howard', 'Howard'),
		new FakeName('Hugh', 'Hughes'),
		new FakeName('Isaac', 'Isaac'),
		new FakeName('Jack', 'Jackson'),
		new FakeName('Jake', 'Jacobs'),
		new FakeName('Jeff', 'Jeffries'),
		new FakeName('Jeremy', 'Jeremiah'),
		new FakeName('Jim', 'James'),
		new FakeName('Joe', 'Joseph'),
		new FakeName('John', 'Johnson'),
		new FakeName('Jordan', 'Jordan'),
		new FakeName('Keith', 'Keith'),
		new FakeName('Kenny', 'Kennedy'),
		new FakeName('Kyle', 'Kyle'),
		new FakeName('Landon', 'Landon'),
		new FakeName('Larry', 'Larson'),
		new FakeName('Lewis', 'Lewis'),
		new FakeName('Marcus', 'Marcis'),
		new FakeName('Mark', 'Marks'),
		new FakeName('Marshall', 'Marshall'),
		new FakeName('Martin', 'Martin'),
		new FakeName('Marvin', 'Marvin'),
		new FakeName('Matt', 'Matthews'),
		new FakeName('Mike', 'Michaels'),
		new FakeName('Mitchell', 'Mitchell'),
		new FakeName('Morgan', 'Morgan'),
		new FakeName('Murray', 'Murray'),
		new FakeName('Neil', 'Neil'),
		new FakeName('Nelson', 'Nelson'),
		new FakeName('Nick', 'Nichols'),
		new FakeName('Oliver', 'Oliver'),
		new FakeName('Owen', 'Owens'),
		new FakeName('Parker', 'Parker'),
		new FakeName('Patrick', 'Patrick'),
		new FakeName('Paul', 'Paul'),
		new FakeName('Pete', 'Peters'),
		new FakeName('Phil', 'Phillips'),
		new FakeName('Ray', 'Ray'),
		new FakeName('Richard', 'Richard'),
		new FakeName('Riley', 'Riley'),
		new FakeName('Ron', 'Reynolds'),
		new FakeName('Ross', 'Ross'),
		new FakeName('Roy', 'Roy'),
		new FakeName('Ryan', 'Ryan'),
		new FakeName('Sam', 'Samuels'),
		new FakeName('Scott', 'Scott'),
		new FakeName('Shane', 'Shane'),
		new FakeName('Spencer', 'Spencer'),
		new FakeName('Stacy', 'Stacy'),
		new FakeName('Steve', 'Stevens'),
		new FakeName('Stewart', 'Stewart'),
		new FakeName('Taylor', 'Taylor'),
		new FakeName('Todd', 'Todd'),
		new FakeName('Tom', 'Thomas'),
		new FakeName('Tony', 'Anthony'),
		new FakeName('Tyler', 'Tyler'),
		new FakeName('Wallace', 'Wallace'),
		new FakeName('Ward', 'Ward'),
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
