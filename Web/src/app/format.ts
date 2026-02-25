export class Format {
	private static timeSecondsFormat: Intl.NumberFormatOptions = { style: 'decimal', minimumFractionDigits: 3, maximumFractionDigits: 3 };
	private static timeSecondsFullFormat: Intl.NumberFormatOptions = { style: 'decimal', minimumIntegerDigits: 2, minimumFractionDigits: 3, maximumFractionDigits: 3 };
	private static leadingTimePartFormat: Intl.NumberFormatOptions = { style: 'decimal', minimumIntegerDigits: 1, maximumFractionDigits: 0 };
	private static timePartFormat: Intl.NumberFormatOptions = { style: 'decimal', minimumIntegerDigits: 2, maximumFractionDigits: 0 };
	private static percentFormat: Intl.NumberFormatOptions = { style: 'percent', minimumFractionDigits: 2, maximumFractionDigits: 2 };
	private static percentWholeFormat: Intl.NumberFormatOptions = { style: 'percent', minimumFractionDigits: 0, maximumFractionDigits: 0 };
	private static tempFormat: Intl.NumberFormatOptions = { style: 'decimal', minimumFractionDigits: 1, maximumFractionDigits: 1 };

	static diff(laps: number, time: number) {
		return laps > 0 ? `${laps} L` : time.toLocaleString(undefined, this.timeSecondsFormat);
	}

	static percent(value: number | null) {
		return value == null ? '' : value.toLocaleString(undefined, this.percentFormat);
	}

	static time(time: number | null) {
		if (time == null)
			return '';
		else if (time < 0)
			return '--:--:--';
		time = Math.round(time);
		let hours = Math.floor(time / (60 * 60));
		let minutes = Math.floor((time - hours * 60 * 60) / 60);
		let seconds = Math.floor(time - hours * 60 * 60 - minutes * 60);
		return `${hours.toLocaleString(undefined, this.timePartFormat)}:${minutes.toLocaleString(undefined, this.timePartFormat)}:${seconds.toLocaleString(undefined, this.timePartFormat)}`;
	}

	static lapTime(laptime?: number) {
		if (typeof laptime != 'number' || laptime <= 0.0)
			return '-';
		let time = Math.round(laptime * 1000) / 1000;
		let hours = Math.floor(time / (60 * 60));
		let minutes = Math.floor((time - hours * 60 * 60) / 60);
		let seconds = time - hours * 60 * 60 - minutes * 60;
		let base = `${seconds.toLocaleString(undefined, this.timeSecondsFullFormat)}`
		if (hours > 0)
			return `${hours.toLocaleString(undefined, this.leadingTimePartFormat)}:${minutes.toLocaleString(undefined, this.timePartFormat)}:${base}`;
		if (minutes > 0)
			return `${minutes.toLocaleString(undefined, this.leadingTimePartFormat)}:${base}`;
		return base;
	}

	static sectorTime(start: number, end: number) {
		if (start < 0.0 || end <= 0.0)
			return '-';
		return this.lapTime(end - start);
	}

	static temp(c: number | null, fahrenheit: boolean = false, long: boolean = false) {
		if (!c)
			return '';
		let mainTemp = fahrenheit ? this.f(c) : c;
		let mainUnit = fahrenheit ? 'F' : 'C';
		let s = `${mainTemp.toLocaleString(undefined, this.tempFormat)} °${mainUnit}`;
		if(!long)
			return s;
		let otherTemp = fahrenheit ? c : this.f(c);
		let otherUnit = fahrenheit ? 'C' : 'F'
		return `${s} (${otherTemp.toLocaleString(undefined, this.tempFormat)} °${otherUnit})`;
	}

	private static f(c: number) {
		return c * 9.0 / 5.0 + 32
	}

	static phase(gamePhase: number | null) {
		switch (gamePhase) {
			case 0: return 'Starting';
			case 1: return 'Reconnaissance laps';
			case 2: return 'Grid';
			case 3: return 'Formation lap';
			case 4: return 'Countdown';
			case 5: return 'Green';
			case 6: return 'FCY';
			case 7: return 'Session stopped';
			case 8: return 'Checkered';
			case 9: return 'Paused';
			default: return `Unknown (${gamePhase})`;
		}
	}

	// indicating pit should be done on lap with entering status
	static status(standing: any) {
		switch (standing.finishStatus) {
			case 'FSTAT_FINISHED':
				return 'Fin';
			case 'FSTAT_CHECKERED':
				return 'Chk';
			case 'FSTAT_DNF':
				return 'DNF';
			case 'FSTAT_DQ':
				return 'DQ';
		}
		if (standing.inGarageStall)
			return 'Gar';
		switch (standing.pitState) {
			case 'NONE':
				return 'Run';
			case 'ENTERING':
				return 'In';
			case 'EXITING':
				return 'Out';
			case 'STOPPED':
				return 'Pit';
			case 'REQUEST':
				return 'Req';
		}
		return typeof(standing.pitState) === 'undefined' && standing.finishStatus == 'FSTAT_NONE' ? 'Run' : '???';
	}

	static relativeTimestamp(now: Date, timestamp: Date) {
		let diff = now.valueOf() - timestamp.valueOf();
		let totalSeconds = diff / 1000;
		if (totalSeconds < 10)
			return 'now';
		else {
			let totalMinutes = totalSeconds / 60;
			if (totalMinutes < 1)
				return `${totalSeconds} seconds ago`;
			else {
				let totalHours = totalMinutes / 60;
				if (totalHours < 1)
					return `${totalMinutes} minutes ago`;
				else {
					let totalDays = totalHours / 24;
					if (totalDays < 1)
						return `${totalHours} hours ago`;
					else if (totalDays < 31)
						return `${totalDays} days ago`;
					else
						return 'long ago';
				}
			}
		}
	}

	static date(date: Date | string | null) {
		if (date == null)
			return;
		if (typeof (date) === 'string') {
			if (!date.endsWith('Z'))
				date += 'Z';
			date = new Date(date);
		}
		let format = (number: number, digits: number) => Math.floor(number).toString().padStart(digits, '0');
		let res = `${format(date.getFullYear(), 4)}-${format(date.getMonth() + 1, 2)}-${format(date.getDate(), 2)} ${format(date.getHours(), 2)}:${format(date.getMinutes(), 2)}`;
		return res;
	}

	static distance(progress: number | null, total: number | null) {
		return progress != null && total != null ? (progress / total).toLocaleString(undefined, this.percentWholeFormat) : null;
	}

	static lapProgress(progress: number | null, total: number | null) {
		if (progress != null && total != null) {
			let value = (progress / total * 10);
			if (value > 10)
				value = 10;
			else if (value < -10)
				value = -10
			return value.toString().split('.')[0];
		}
		return '0';
	}

	static speed(v: number, unit: string) {
		if(unit == 'mph')
			v *= 2.237;
		else if(unit == 'km/h')
			v *= 3.6;
		return v.toLocaleString(undefined, this.tempFormat);
	}
}
