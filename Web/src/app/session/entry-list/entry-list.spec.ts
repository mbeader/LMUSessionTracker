import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

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
