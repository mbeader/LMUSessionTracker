import { Component, Input } from '@angular/core';
import { NgbPopover } from '@ng-bootstrap/ng-bootstrap';
import { Lap, Pit } from '../../tracking';
import { Format } from '../../format';

@Component({
	selector: 'app-cars-tire-badge',
	imports: [NgbPopover],
	templateUrl: './tire-badge.component.html',
	styleUrl: './tire-badge.component.css',
})
export class TireBadgeComponent {
	@Input() pit?: Pit;
	@Input() lap?: Lap;
	@Input() compounds?: string[];
	@Input() align?: string;
	protected tires: Tires = new Tires();
	protected Format = Format;

	ngOnChanges() {
		this.tires = this.getTires();
	}

	private getTires() {
		return new Tires(this.pit, this.lap, this.compounds);
	}

	getUsage() {
		if (this.lap && (!this.pit || this.pit.lap < this.lap.lapNumber))
			return this.lap as TireUsage;
		else if (this.pit)
			return this.pit as TireUsage;
		else
			return undefined;
	}
}

type TireUsage = {
	lfUsage: number;
	rfUsage: number;
	lrUsage: number;
	rrUsage: number;
};

export class Tires {
	public readonly lf: Tire;
	public readonly rf: Tire;
	public readonly lr: Tire;
	public readonly rr: Tire;
	public readonly all?: Tire;

	constructor(pit?: Pit, lap?: Lap, compounds?: (string | undefined)[]) {
		if (pit && lap) {
			if (lap.lfCompound || lap.rfCompound || lap.lrCompound || lap.rrCompound) {
				let same = true;
				same = same && pit.lfCompound == lap.lfCompound;
				same = same && pit.rfCompound == lap.rfCompound;
				same = same && pit.lrCompound == lap.lrCompound;
				same = same && pit.rrCompound == lap.rrCompound;
				if (!same) {
					pit = undefined;
				}
			}
		}
		if (!pit && lap && !compounds) {
			compounds = [lap.lfCompound, lap.rfCompound, lap.lrCompound, lap.rrCompound];
		}
		this.lf = new Tire(pit?.lfCompound, pit?.lfChanged, pit?.lfNew, compounds?.at(0));
		this.rf = new Tire(pit?.rfCompound, pit?.rfChanged, pit?.rfNew, compounds?.at(1));
		this.lr = new Tire(pit?.lrCompound, pit?.lrChanged, pit?.lrNew, compounds?.at(2));
		this.rr = new Tire(pit?.rrCompound, pit?.rrChanged, pit?.rrNew, compounds?.at(3));
		if (this.lf.same(this.rf) && this.lf.same(this.lr) && this.lf.same(this.rr))
			this.all = this.lf;
	}

	getDescription() {
		if (this.all) {
			return this.all.getDescription();
		}
		let lf = false, rf = false, lr = false, rr = false;
		let desc = [];
		if (this.lf.same(this.rf)) {
			desc.push(`Fronts: ${this.lf.getDescription()}`);
			lf = true;
			rf = true;
		}
		if (this.lr.same(this.rr)) {
			desc.push(`Rears: ${this.lr.getDescription()}`);
			lr = true;
			rr = true;
		}
		if (desc.length == 0) {
			if (this.lf.same(this.lr)) {
				desc.push(`Lefts: ${this.lf.getDescription()}`);
				lf = true;
				lr = true;
			}
			if (this.rf.same(this.rr)) {
				desc.push(`Rights: ${this.rf.getDescription()}`);
				rf = true;
				rr = true;
			}
		}
		if (!lf)
			desc.push(`LF: ${this.lf.getDescription()}`);
		if (!rf)
			desc.push(`RF: ${this.rf.getDescription()}`);
		if (!lr)
			desc.push(`LR: ${this.lr.getDescription()}`);
		if (!rr)
			desc.push(`RR: ${this.rr.getDescription()}`);
		return desc.join(', ');
	}
}

export class Tire {
	public readonly compound: string;
	public readonly type: string;

	constructor(compound?: string | null, changed?: boolean, notUsed?: boolean, compoundFallback?: string) {
		let resolvedCompound = compound ? compound : compoundFallback;
		switch (resolvedCompound) {
			case 'Soft':
			case 'Medium':
			case 'Hard':
			case 'Wet':
				this.compound = resolvedCompound;
				break;
			default:
				this.compound = '';
				break;
		}
		if (this.compound && compound && typeof changed === 'boolean' && typeof notUsed === 'boolean')
			this.type = notUsed ? 'New' : !changed ? 'Unchanged' : 'Used';
		else
			this.type = 'Unknown';
	}

	same(other: Tire) {
		return this.compound == other.compound && this.type == other.type;
	}

	getDescription() {
		return this.compound ? `${this.type}${this.type ? ' ' : ''}${this.compound}` : this.type;
	}
}
