import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Tire, TireBadgeComponent, Tires } from './tire-badge.component';
import { Pit } from '../../tracking';

describe('TireBadgeComponent', () => {
	let component: TireBadgeComponent;
	let fixture: ComponentFixture<TireBadgeComponent>;

	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [TireBadgeComponent]
		})
			.compileComponents();

		fixture = TestBed.createComponent(TireBadgeComponent);
		component = fixture.componentInstance;
		await fixture.whenStable();
	});

	it('should create', () => {
		expect(component).toBeTruthy();
	});
});

describe('Tire', () => {
	describe('getDescription', () => {
		test.each([
			{ compound: 'Soft', changed: true, notUsed: true, ex: 'New Soft' },
			{ compound: 'Medium', changed: true, notUsed: true, ex: 'New Medium' },
			{ compound: 'Hard', changed: true, notUsed: true, ex: 'New Hard' },
			{ compound: 'Wet', changed: true, notUsed: true, ex: 'New Wet' },
			{ compound: 'Soft', changed: true, notUsed: false, ex: 'Used Soft' },
			{ compound: 'Soft', changed: false, notUsed: false, ex: 'Unchanged Soft' },
			{ compound: 'Soft', changed: false, notUsed: true, ex: 'New Soft' },
			{ compound: 'N/A', changed: true, notUsed: true, ex: 'Unknown' },
			{ compound: 'N/A', changed: false, notUsed: true, ex: 'Unknown' },
			{ compound: 'N/A', changed: true, notUsed: false, ex: 'Unknown' },
			{ compound: 'N/A', changed: false, notUsed: false, ex: 'Unknown' },
			{ compound: 'Soft', changed: undefined, notUsed: true, ex: 'Unknown Soft' },
			{ compound: 'Soft', changed: true, notUsed: undefined, ex: 'Unknown Soft' },
			{ compound: 'Soft', changed: undefined, notUsed: undefined, ex: 'Unknown Soft' },
		])('$compound $changed $notUsed', ({ compound, changed, notUsed, ex }) => {
			expect(new Tire(compound, changed, notUsed).getDescription()).toBe(ex);
		});
	});
});

type PitTire = {
	compound: string | null;
	changed: boolean;
	notUsed: boolean;
};

describe('Tires', () => {
	let pit = (pit?: { lf: PitTire, rf: PitTire, lr: PitTire, rr: PitTire }) => {
		let p = {} as Pit;
		if (pit) {
			p.lfCompound = pit.lf.compound;
			p.lfChanged = pit.lf.changed;
			p.lfNew = pit.lf.notUsed;
			p.rfCompound = pit.rf.compound;
			p.rfChanged = pit.rf.changed;
			p.rfNew = pit.rf.notUsed;
			p.lrCompound = pit.lr.compound;
			p.lrChanged = pit.lr.changed;
			p.lrNew = pit.lr.notUsed;
			p.rrCompound = pit.rr.compound;
			p.rrChanged = pit.rr.changed;
			p.rrNew = pit.rr.notUsed;
		}
		return p;
	}
	describe('getDescription', () => {
		test.each([
			{
				tires: {
					lf: { compound: 'Soft', changed: true, notUsed: true },
					rf: { compound: 'Soft', changed: true, notUsed: true },
					lr: { compound: 'Soft', changed: true, notUsed: true },
					rr: { compound: 'Soft', changed: true, notUsed: true },
				}, ex: 'New Soft'
			},
			{
				tires: {
					lf: { compound: 'Soft', changed: true, notUsed: true },
					rf: { compound: 'Soft', changed: true, notUsed: true },
					lr: { compound: 'Soft', changed: false, notUsed: false },
					rr: { compound: 'Soft', changed: false, notUsed: false },
				}, ex: 'Fronts: New Soft, Rears: Unchanged Soft'
			},
			{
				tires: {
					lf: { compound: 'Soft', changed: true, notUsed: false },
					rf: { compound: 'Soft', changed: false, notUsed: false },
					lr: { compound: 'Soft', changed: true, notUsed: false },
					rr: { compound: 'Soft', changed: false, notUsed: false },
				}, ex: 'Lefts: Used Soft, Rights: Unchanged Soft'
			},
			{
				tires: {
					lf: { compound: 'Soft', changed: true, notUsed: true },
					rf: { compound: 'Medium', changed: true, notUsed: true },
					lr: { compound: 'Hard', changed: true, notUsed: true },
					rr: { compound: 'Wet', changed: true, notUsed: true },
				}, ex: 'LF: New Soft, RF: New Medium, LR: New Hard, RR: New Wet'
			},
		])('$ex', ({ tires, ex }) => {
			expect(new Tires(pit(tires)).getDescription()).toBe(ex);
		});
	});
});
