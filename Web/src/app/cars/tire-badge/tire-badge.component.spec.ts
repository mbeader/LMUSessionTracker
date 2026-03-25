import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Tire, TireBadgeComponent, Tires } from './tire-badge.component';
import { Lap, Pit } from '../../tracking';

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
	};
	let pit2 = (pit2?: { lf: string, rf: string, lr: string, rr: string }) => {
		return pit({
			lf: { compound: pit2?.lf ?? null, changed: true, notUsed: true },
			rf: { compound: pit2?.rf ?? null, changed: true, notUsed: true },
			lr: { compound: pit2?.lr ?? null, changed: true, notUsed: true },
			rr: { compound: pit2?.rr ?? null, changed: true, notUsed: true },
		});
	};
	let lap = (lap?: { lf?: string, rf?: string, lr?: string, rr?: string }) => {
		let l = {} as Lap;
		if (lap) {
			l.lfCompound = lap.lf;
			l.rfCompound = lap.rf;
			l.lrCompound = lap.lr;
			l.rrCompound = lap.rr;
		}
		return l;
	};

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

		test.each([
			{
				name: 'undefined lap tires',
				pitTires: { lf: 'Soft', rf: 'Soft', lr: 'Soft', rr: 'Soft' },
				lapTires: { lf: undefined, rf: undefined, lr: undefined, rr: undefined },
				ex: 'New Soft'
			},
			{
				name: 'matching lap tires',
				pitTires: { lf: 'Soft', rf: 'Soft', lr: 'Soft', rr: 'Soft' },
				lapTires: { lf: 'Soft', rf: 'Soft', lr: 'Soft', rr: 'Soft' },
				ex: 'New Soft'
			},
			{
				name: 'unmatching lap lf',
				pitTires: { lf: 'Soft', rf: 'Soft', lr: 'Soft', rr: 'Soft' },
				lapTires: { lf: 'Medium', rf: 'Soft', lr: 'Soft', rr: 'Soft' },
				ex: 'Rears: Unknown Soft, LF: Unknown Medium, RF: Unknown Soft'
			},
			{
				name: 'unmatching lap rf',
				pitTires: { lf: 'Soft', rf: 'Soft', lr: 'Soft', rr: 'Soft' },
				lapTires: { lf: 'Soft', rf: 'Medium', lr: 'Soft', rr: 'Soft' },
				ex: 'Rears: Unknown Soft, LF: Unknown Soft, RF: Unknown Medium'
			},
			{
				name: 'unmatching lap lr',
				pitTires: { lf: 'Soft', rf: 'Soft', lr: 'Soft', rr: 'Soft' },
				lapTires: { lf: 'Soft', rf: 'Soft', lr: 'Medium', rr: 'Soft' },
				ex: 'Fronts: Unknown Soft, LR: Unknown Medium, RR: Unknown Soft'
			},
			{
				name: 'unmatching lap rr',
				pitTires: { lf: 'Soft', rf: 'Soft', lr: 'Soft', rr: 'Soft' },
				lapTires: { lf: 'Soft', rf: 'Soft', lr: 'Soft', rr: 'Medium' },
				ex: 'Fronts: Unknown Soft, LR: Unknown Soft, RR: Unknown Medium'
			},
		])('$name', ({ name, pitTires, lapTires, ex }) => {
			expect(new Tires(pit2(pitTires), lap(lapTires)).getDescription()).toBe(ex);
		});
	});
});
