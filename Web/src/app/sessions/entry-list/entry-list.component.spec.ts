import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ServerApiServiceToken } from '../../data/server-api/server-api.service';
import { ServerLiveServiceToken } from '../../data/server-live/server-live.service';

import { EntryListComponent } from './entry-list.component';

describe('EntryListComponent', () => {
	let component: EntryListComponent;
	let fixture: ComponentFixture<EntryListComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [EntryListComponent],
			providers: [
				{ provide: ChangeDetectorRef, useValue: {} },
				{ provide: ActivatedRoute, useValue: { snapshot: { paramMap: new Map() } } },
				{ provide: ServerApiServiceToken, useValue: { } },
				{ provide: ServerLiveServiceToken, useValue: { } },
			]
		}).compileComponents();

		fixture = TestBed.createComponent(EntryListComponent);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});
