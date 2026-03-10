import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbPopover } from '@ng-bootstrap/ng-bootstrap';
import { ServerApiService, ServerApiServiceToken } from '../../server-api.service/server-api.service';
import { ServerLiveService, ServerLiveServiceToken } from '../../server-live.service/server-live.service';
import { Car } from '../../models';
import { ChatMessage, ChatViewModel } from '../../view-models';
import { Format } from '../../format';
import { getFlag } from '../../utils';
import { ClassBadge } from '../class-badge/class-badge';

@Component({
	selector: 'app-session-chat',
	imports: [NgbPopover, ClassBadge],
	templateUrl: './chat.html',
	styleUrl: './chat.css',
})
export class Chat {
	private ref = inject(ChangeDetectorRef);
	private route = inject(ActivatedRoute);
	private api = inject(ServerApiServiceToken);
	private live = inject(ServerLiveServiceToken);
	cars = new Map<string, Car[]>();
	messages: ChatMessageData[] = [];
	now: Date = new Date();
	Format = Format;
	Utils = { getFlag };

	constructor() {
		let sessionId = this.route.snapshot.paramMap.get('sessionId');
		if (!sessionId)
			return;
		this.api.getEntryList(sessionId).then(result => {
			if (result) {
				for (let car of result) {
					if (car?.entry?.members) {
						for (let member of car.entry.members) {
							let names = [];
							let space = member.name.indexOf(' ');
							while (space > 0) {
								names.push(`${member.name[0]} ${member.name.substring(space + 1)}`);
								space = member.name.indexOf(' ', space + 1);
							}

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
			this.api.getChat(sessionId).then(result => {
				this.updateChat(result ?? new ChatViewModel());
				this.live.joinChat(sessionId, this.updateChat.bind(this));
			}, error => { console.log(error); })
		}, error => { console.log(error); })
		setInterval(() => this.now = new Date(), 10000);
	}

	updateChat(chat: ChatViewModel) {
		if (!chat.append)
			this.messages.length = 0;
		for (let message of chat.chat) {
			let msg = new ChatMessageData(message);
			msg.findSender(this.cars);
			this.messages.push(msg);
		}
		this.ref.markForCheck();
	}

	coalesce(s: string | null | undefined) {
		return s ?? '\u00A0';
	}
}

class ChatMessageData {
	chat: ChatMessage;
	date: Date;
	message: string;
	sender?: string;
	car?: Car[];

	constructor(chat: ChatMessage) {
		this.chat = chat;
		this.date = new Date(chat.timestamp);
		this.message = chat.message;
	}

	findSender(carsMap: Map<string, Car[]>) {
		let parts = this.message.split(':');
		if (parts.length < 2)
			return;
		let cars = carsMap.get(parts[0]);
		if (cars) {
			this.sender = parts[0];
			this.message = this.message.substring(this.sender.length + 1);
			this.car = cars;
		}
	}
}
