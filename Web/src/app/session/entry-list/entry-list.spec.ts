import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ServerApiServiceToken } from '../../server-api.service/server-api.service';
import { ServerLiveServiceToken } from '../../server-live.service/server-live.service';

import { EntryList } from './entry-list';

describe('EntryList', () => {
	let component: EntryList;
	let fixture: ComponentFixture<EntryList>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [EntryList],
			providers: [
				{ provide: ChangeDetectorRef, useValue: {} },
				{ provide: ActivatedRoute, useValue: { snapshot: { paramMap: new Map() } } },
				{ provide: ServerApiServiceToken, useValue: { } },
				{ provide: ServerLiveServiceToken, useValue: { } },
			]
		}).compileComponents();

		fixture = TestBed.createComponent(EntryList);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
