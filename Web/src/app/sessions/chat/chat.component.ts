import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbPopover } from '@ng-bootstrap/ng-bootstrap';
import { ServerApiService, ServerApiServiceToken } from '../../data/server-api/server-api.service';
import { ServerLiveService, ServerLiveServiceToken } from '../../data/server-live/server-live.service';
import { ChatMessage, ChatViewModel, SessionEntry } from '../../view-models';
import { Format } from '../../format';
import { coalesce, getBadge, getFlag } from '../../utils';
import { ClassBadgeComponent } from '../../cars/class-badge/class-badge.component';
import { BrandBadgeComponent } from '../../cars/brand-badge/brand-badge.component';

@Component({
	selector: 'app-sessions-chat',
	imports: [NgbPopover, ClassBadgeComponent, BrandBadgeComponent],
	templateUrl: './chat.component.html',
	styleUrl: './chat.component.css',
})
export class ChatComponent {
	private ref = inject(ChangeDetectorRef);
	private route = inject(ActivatedRoute);
	private api = inject(ServerApiServiceToken);
	private live = inject(ServerLiveServiceToken);
	private sessionId!: string;
	cars = new Map<string, SessionEntry[]>();
	messages: ChatMessageData[] = [];
	now: Date = new Date();
	Format = Format;
	Utils = { getFlag, getBadge };
	coalesce = coalesce;

	constructor() {
		let sessionId = this.route.snapshot.paramMap.get('sessionId');
		if (!sessionId)
			return;
		this.sessionId = sessionId;
		this.updateEntries(sessionId).then(() => {
			this.api.getChat(sessionId).then(result => {
				this.updateChat(result ?? new ChatViewModel());
				this.live.joinChat(sessionId, this.updateChat.bind(this));
			}, error => { console.log(error); })
		}, error => { console.log(error); })
		setInterval(() => this.now = new Date(), 10000);
	}

	private updateEntries(sessionId: string) {
		return this.api.getEntryList(sessionId).then(result => {
			if (result) {
				this.cars.clear();
				for (let car of result) {
					if (car?.entry?.members) {
						for (let member of car.entry.members) {
							let names = ChatComponent.generatePossibleSenderNames(member.name);

							for (let name of names) {
								let cars = this.cars.get(name);
								if (!cars) {
									cars = [];
									this.cars.set(name, cars);
								}
								if (!cars.includes(car))
									cars.push(car);
							}
						}
					}
				}
			}
		}, error => { console.log(error); })
	}

	updateChat(chat: ChatViewModel) {
		if (!chat.append)
			this.messages.length = 0;
		let senderMissing = false;
		for (let message of chat.chat) {
			let msg = new ChatMessageData(message);
			senderMissing = senderMissing || msg.findSender(this.cars);
			this.messages.push(msg);
		}
		this.ref.markForCheck();
		if (senderMissing)
			this.updateEntries(this.sessionId).then(() => {
				for (let message of this.messages)
					message.findSender(this.cars);
				this.ref.markForCheck();
			});
	}

	static generatePossibleSenderNames(name: string) {
		let names: string[] = [];
		let space = name.indexOf(' ');
		let initials = `${name[0]} `;
		if (space > 0) {
			while (space > 0) {
				let nameSuffix = name.substring(space + 1);
				let senderName = `${name[0]} ${nameSuffix}`;
				this.addName(names, senderName);
				if (initials.length > 2) {
					let initialsName = `${initials}${nameSuffix}`;
					this.addName(names, initialsName);
				}
				initials = `${initials}${nameSuffix[0]} `;
				space = name.indexOf(' ', space + 1);
			}
		} else
			names.push(name);
		return names;
	}

	private static addName(names: string[], name: string) {
		if (!names.includes(name))
			names.push(name);
		if (name.length > 15) {
			let truncated = name.substring(0, 15);
			if (!names.includes(truncated))
				names.push(truncated);
		}
	}
}

class ChatMessageData {
	chat: ChatMessage;
	date: Date;
	message: string;
	sender?: string;
	car?: SessionEntry[];

	constructor(chat: ChatMessage) {
		this.chat = chat;
		this.date = new Date(chat.timestamp);
		this.message = chat.message;
	}

	findSender(carsMap: Map<string, SessionEntry[]>) {
		let parts = this.message.split(':');
		if (parts.length < 2)
			return false;
		let cars = carsMap.get(parts[0]);
		if (cars) {
			this.sender = parts[0];
			this.message = this.message.substring(this.sender.length + 1);
			this.car = cars;
			return false;
		}
		return true;
	}
}
