

export function whenExists<T extends Element>(selector: string, callback: (el: T) => void) {
	new Promise<T>((resolve) => {
		let fn = (fn: any) => {
			let el = document.querySelector<T>(selector);
			if (!el)
				setTimeout(fn, 100);
			else
				resolve(el);
		}
		fn(fn.bind(null, fn));
	}).then(callback);
}

export function classId(carClass: string) {
	switch (carClass) {
		case 'Hyper': return 'hy';
		case 'LMP2_ELMS': return 'p2e';
		case 'LMP2': return 'p2';
		case 'LMP3': return 'p3';
		case 'GTE': return 'gte';
		case 'GT3': return 'gt3';
		default: return '';
	}
}

export function statusClass(status: string) {
	return status == 'Run' ? 'text-success' : status == 'Req' ? 'text-info' : status == 'Out' ? 'text-warning' : status == 'In' || status == 'Pit' ? 'text-danger' : 'text-secondary';
}

/**
 * Converts team member nationality (mostly ISO 3166-1-alpha-2) to available flag icon
 */
export function getFlag(country: string) {
	if (!country)
		return 'none';
	country = country.toLowerCase();
	switch (country) {
		case 'ac':
			return 'sh-ac';
		case 'ea':
			return 'es';
		case 'sh':
			return 'sh-hl';
		case 'ta':
			return 'sh-ta';
	}
	return country;
}
