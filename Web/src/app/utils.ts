

export function whenExists<T extends Element>(selector: string, callback: (el: T) => void) {
	new Promise<T>((resolve) => {
		let fn = (fn: any) => {
			let el = document.querySelector<T>(selector);
			if (!el)
				setTimeout(fn, 100);
			else
				resolve(el);
		}
		fn(fn);
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
