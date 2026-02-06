import { ChangeDetectorRef, Component, HostListener, inject } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subject, debounceTime } from 'rxjs';
import { ServerApiService } from '../../server-api.service';
import { ServerLiveService } from '../../server-live.service';
import { classId, whenExists } from '../../utils';
import { Format } from '../../format';
import { SessionViewModel } from '../../view-models';
import { TrackMapPoint } from '../../lmu';
import { Car, CarKey } from '../../tracking';

@Component({
	selector: 'app-session-track-map',
	imports: [],
	templateUrl: './track-map.html',
	styleUrl: './track-map.css',
})
export class TrackMap {
	private ref = inject(ChangeDetectorRef);
	private route = inject(ActivatedRoute);
	private api = inject(ServerApiService);
	private live = inject(ServerLiveService);
	private resize = new Subject();
	private mousemove = new Subject<MousePosition>();
	private map: TrackMapService | null = null
	private trackMapPoints: TrackMapPoint[] = [];
	session: SessionViewModel | null = null;
	hasStandings: boolean = false;
	Format = Format;

	constructor() {
		let sessionId = this.route.snapshot.paramMap.get('sessionId');
		if (!sessionId)
			return;
		this.api.getTrackMap(sessionId).then(result => {
			this.trackMapPoints = result ?? [];
			this.api.getSession(sessionId).then(result => {
				this.session = result;
				this.hasStandings = this.session.standings != null && this.session.standings.length > 0;
				this.ref.markForCheck();
				whenExists('#map-wrapper', this.initMap.bind(this));
			}, error => { console.log(error); })
		}, error => { console.log(error); })
	}

	initMap(wrapper: HTMLDivElement) {
		this.map = new TrackMapService(wrapper, this.resize, this.mousemove);
		this.map.map(this.trackMapPoints);
		if (this.hasStandings && this.session?.session?.sessionId) {
			this.map.veh(Veh.from(this.session));
			this.live.joinLive(this.session.session.sessionId, this.updateSession.bind(this));
		}
	}

	updateSession(session: SessionViewModel) {
		if (this.session) {
			SessionViewModel.merge(this.session, session);
			if (this.map) {
				this.map.veh(Veh.from(session));
			}
			this.ref.markForCheck();
		}
	}

	@HostListener('window:resize', ['$event.target'])
	public onResize(target: EventTarget | null) {
		if (target instanceof Window)
			this.resize.next({ width: target.innerWidth, height: target.innerHeight });
	}

	@HostListener('window:mousemove', ['$event'])
	public onMousemove(e: MouseEvent) {
		this.mousemove.next({ x: e.clientX, y: e.clientY });
	}
}

type MousePosition = {
	x: number;
	y: number;
}

class Veh {
	x: number;
	y: number;
	c: string;
	pic: number;
	driver: string;
	car?: Car;

	constructor(x: number, y: number, c: string, pic: number, driver: string, car?: Car) {
		this.x = x;
		this.y = y;
		this.c = c;
		this.driver = driver;
		this.pic = pic;
		this.car = car;
	}

	static from(session: SessionViewModel) {
		let entries: Map<string, Car> = new Map();
		let positionInClass: Map<string, number> = new Map();
		if (session && session.entries) {
			for (let car in session.entries) {
				entries.set(car ?? '', session.entries[car]);
			}
		}
		if (session && session.positionInClass) {
			for (let car in session.positionInClass) {
				positionInClass.set(car ?? '', session.positionInClass[car]);
			}
		}
		let vehs: Veh[] = [];
		for (let standing of session.standings ?? []) {
			let id = CarKey.fromStanding(standing).id;
			let car = entries.get(id);
			vehs.push(new Veh(standing.carPosition.x, standing.carPosition.z, classId(standing.carClass), positionInClass.get(id) ?? 0, standing.driverName, car));
		}
		return vehs;
	}
}

class Point {
	private point: TrackMapPoint;

	constructor(point: TrackMapPoint) {
		this.point = point;
	}

	get x() { return this.point.x; }
	get y() { return this.point.z; }
	get z() { return this.point.y; }
	get type() { return this.point.type; }

	set x(v) { this.point.x = v; }
	set y(v) { this.point.z = v; }
	set z(v) { this.point.y = v; }
	set type(v) { this.point.type = v; }
}

class Track {
	points: Point[] = [];
	pit: Point[] = [];
	s1: Point[] = [];
	s2: Point[] = [];
	s3: Point[] = [];
	maxx: number = 0;
	maxy: number = 0;
	minx: number = 0;
	miny: number = 0;

	hasSectors() {
		return this.s1.length > 0 && this.s2.length > 0 && this.s3.length > 0;
	}
}

class TrackMapService {
	private wrapper: HTMLDivElement;
	private canvas: HTMLCanvasElement;
	private context: CanvasRenderingContext2D;
	private track?: Track;
	private dy = 0;
	private dx = 0;
	private cy = 0;
	private cx = 0;
	private dim = 0
	private scalefactor = 1;
	private scaled = { dx: 0, dy: 0, cx: 0, cy: 0 };
	private classcolors: { [key: string]: { color: string, text: string } } = {
		hy: { color: '#e21e19', text: 'white' },
		p2: { color: '#005096', text: 'white' },
		p2e: { color: '#e95b0c', text: 'white' },
		p3: { color: '#411c52', text: 'white' },
		gte: { color: '#f08c00', text: 'white' },
		gt3: { color: '#009639', text: 'white' },
		default: { color: 'gray', text: 'white' }
	};
	private edges = { maxx: Number.MIN_SAFE_INTEGER, minx: Number.MAX_SAFE_INTEGER, maxy: Number.MIN_SAFE_INTEGER, miny: Number.MAX_SAFE_INTEGER };
	private staticcanvas: { canvas: HTMLCanvasElement, context: CanvasRenderingContext2D };
	private mouse: MousePosition = { x: 0, y: 0 };
	private lastVehs: Veh[] = [];

	constructor(wrapper: HTMLDivElement, resize: Subject<unknown>, mousemove: Subject<MousePosition>) {
		let canvas = wrapper.querySelector<HTMLCanvasElement>('canvas');
		if (!canvas)
			throw new Error('Missing map canvas');
		this.wrapper = wrapper;
		this.canvas = canvas;

		let staticcanvas: any = new Object();
		staticcanvas.canvas = document.createElement('canvas');
		staticcanvas.context = staticcanvas.canvas.getContext('2d');
		this.staticcanvas = { canvas: staticcanvas.canvas, context: staticcanvas.context };

		let context = this.canvas.getContext('2d');
		if (!context)
			throw new Error('Could not get context');
		this.context = context;
		//if (window.location.pathname == '/map') {
		//this.resizeMapMap();
		//window.addEventListener('resize', this.resizeMapMap.bind(this), true);
		//} else {
		//	this.resizeMapHome();
		//	window.addEventListener('resize', this.resizeMapHome.bind(this), true);
		//}
		this.staticcanvas.canvas.width = this.canvas.width;
		this.staticcanvas.canvas.height = this.canvas.height;
		context = this.staticcanvas.canvas.getContext('2d');
		if (!context)
			throw new Error('Could not get context');
		this.staticcanvas.context = context;

		this.resizeMapMap();
		resize.pipe(debounceTime(500)).subscribe(this.resizeMapMap.bind(this));
		mousemove.subscribe(pos => { this.mouse = pos; this.veh(this.lastVehs); });
	}

	veh(vehs: Veh[]) {
		if (!this.context)
			return;
		this.context.clearRect(0, 0, this.canvas.width, this.canvas.height);
		this.context.drawImage(this.staticcanvas.canvas, 0, 0);

		let bounds = this.canvas.getBoundingClientRect();
		let mouseX = this.mouse.x - bounds.left;
		let mouseY = this.mouse.y - bounds.top;

		let changed = false;
		let hoverIndex = -1;
		for (let i = vehs.length - 1; i >= 0; i--) {
			let veh = vehs[i];
			let colors = this.classcolors[veh.c] ?? this.classcolors['default'];
			if ((this.track?.points.length ?? 0) == 0) {
				if (this.edges.maxx < veh.x) {
					this.edges.maxx = veh.x;
					changed = true;
				} else if (this.edges.minx > veh.x) {
					this.edges.minx = veh.x;
					changed = true;
				} if (this.edges.maxy < veh.y) {
					this.edges.maxy = veh.y;
					changed = true;
				} else if (this.edges.miny > veh.y) {
					this.edges.miny = veh.y;
					changed = true;
				}
			}
			let x = this.calcX(veh.x - this.cx);
			let y = this.calcY(-1 * veh.y + this.cy);
			this.context.strokeStyle = 'black';
			this.context.fillStyle = colors.color;
			this.context.lineWidth = 2;
			this.context.beginPath();
			this.context.arc(x, y, 10, 0, Math.PI * 2, true);
			this.context.stroke();
			this.context.fill();
			this.context.fillStyle = colors.text;
			this.context.font = '12px serif';
			this.context.textAlign = 'center';
			this.context.fillText((i + 1).toString(), x, y + 4);
			if (this.context.isPointInPath(mouseX, mouseY))
				hoverIndex = i;
		};
		if (changed) {
			this.dx = this.edges.maxx - this.edges.minx;
			this.dy = this.edges.maxy - this.edges.miny;
			this.cx = (this.edges.maxx + this.edges.minx) / 2;
			this.cy = (this.edges.maxy + this.edges.miny) / 2;
			this.calcScaleFactor(this.dx, this.dy);
		}
		if (hoverIndex >= 0) {
			let veh = vehs[hoverIndex];
			this.context.textAlign = 'left';
			this.context.fillText(`${hoverIndex + 1} (${veh.pic}) ${veh.driver} - ${veh.car?.class} #${veh.car?.number} ${veh.car?.teamName}`, 0, 10);
		}
		this.lastVehs = vehs;
	}

	map(map: TrackMapPoint[]) {
		this.track = new Track();
		for (let point of map) {
			let arr = point.type == 0 ? this.track.points : point.type == 1 ? this.track.pit : null;
			if (arr)
				arr.push(new Point(point));
		}
		if (this.track.points.length > 0) {
			this.track.maxx = this.track.points[0].x;
			this.track.maxy = this.track.points[0].y;
			this.track.minx = this.track.points[0].x;
			this.track.miny = this.track.points[0].y;
		}
		for (let p of this.track.points) {
			if (p.x > this.track.maxx)
				this.track.maxx = p.x;
			else if (p.x < this.track.minx)
				this.track.minx = p.x;
			if (p.y > this.track.maxy)
				this.track.maxy = p.y;
			else if (p.y < this.track.miny)
				this.track.miny = p.y;
		}
		this.dx = this.track.maxx - this.track.minx;
		this.dy = this.track.maxy - this.track.miny;
		this.cx = (this.track.maxx + this.track.minx) / 2;
		this.cy = (this.track.maxy + this.track.miny) / 2;
		if (this.track.points.length > 0)
			this.processTrack(this.track);
		if (this.track.pit.length > 0)
			this.processPoints(this.track.pit);
		this.calcScaleFactor(this.dx, this.dy);
		this.context.clearRect(0, 0, this.canvas.width, this.canvas.height);
		this.drawTrack();
	}

	clear() {
		this.context.clearRect(0, 0, this.canvas.width, this.canvas.height);
		this.staticcanvas.context.clearRect(0, 0, this.canvas.width, this.canvas.height);
	}

	private processTrack(track: Track) {
		let sectors = track.hasSectors() ? [track.s1, track.s2, track.s3] : [track.points];
		for (let i = 0; i < sectors.length; i++) {
			this.processPoints(sectors[i]);
		}
		track.maxx = sectors[0][0].x;
		track.minx = sectors[0][0].x;
		track.maxy = sectors[0][0].y;
		track.miny = sectors[0][0].y;
		for (let i = 0; i < sectors.length; i++) {
			this.recalcSectorLimits(track, sectors[i]);
		}
	}

	private processPoints(sector: Point[]) {
		for (let i = 0; i < sector.length; i++)
			this.processPoint(sector[i]);
	}

	private processPoint(point: Point) {
		point.x -= this.cx;
		point.y *= -1;
		point.y += this.cy;
	}

	private recalcSectorLimits(track: Track, sector: Point[]) {
		for (let i = 0; i < sector.length; i++) {
			if (sector[i].x > track.maxx)
				track.maxx = sector[i].x;
			else if (sector[i].x < track.minx)
				track.minx = sector[i].x;
			if (sector[i].y > track.maxy)
				track.maxy = sector[i].y;
			else if (sector[i].y < track.miny)
				track.miny = sector[i].y;
		}
	}

	private resizeMapMap() {
		let tempwidth = this.wrapper.offsetWidth;
		if (document.documentElement.scrollHeight > document.documentElement.clientHeight)
			while (document.documentElement.scrollHeight > document.documentElement.clientHeight)
				this.canvas.height--;
		else {
			do {
				this.canvas.height++;
			} while (document.documentElement.scrollHeight == document.documentElement.clientHeight);
		}
		let size = tempwidth > this.canvas.height ? this.canvas.height : tempwidth;
		size -= 2;
		this.resizeMap(size, size);
	}

	private resizeMapHome() {
		this.resizeMap(this.wrapper.offsetWidth, this.canvas.width > 400 ? 400 : this.canvas.width);
	}

	private resizeMap(width: number, height: number) {
		this.canvas.width = width;
		this.canvas.height = height;
		this.staticcanvas.canvas.width = this.canvas.width;
		this.staticcanvas.canvas.height = this.canvas.height;
		this.dim = this.canvas.height / 2;
		if ((this.track?.points.length ?? 0) > 0) {
			this.calcScaleFactor(this.dx, this.dy);
			this.drawTrack();
		}
	}

	private drawTrack() {
		this.staticcanvas.context.clearRect(0, 0, this.canvas.width, this.canvas.height);
		if (this.track && (this.track.points.length ?? 0) > 0) {
			this.staticcanvas.context.lineWidth = 15;
			if (this.track.hasSectors()) {
				this.drawSector(this.track.s1, this.track.s2[0], 'navy');
				this.drawSector(this.track.s2, this.track.s3[0], 'green');
				this.drawSector(this.track.s3, this.track.s1[0], 'maroon');

				this.drawChevrons(this.track.s3[this.track.s3.length - 1], this.track.s1[0], this.track.s1[1]);

				this.drawSectorMarker(this.track.s3[this.track.s3.length - 1], this.track.s1[0], this.track.s1[1], 'white', 'black');
				this.drawSectorMarker(this.track.s1[this.track.s1.length - 1], this.track.s2[0], this.track.s2[1], 'gray');
				this.drawSectorMarker(this.track.s2[this.track.s2.length - 1], this.track.s3[0], this.track.s3[1], 'gray');
			} else {
				if (this.track.pit.length > 0)
					this.drawSector(this.track.pit, this.track.pit[this.track.pit.length - 1], '#343a40');
				this.drawSector(this.track.points, this.track.points[0], '#6c757d');
			}
		}
		this.context.drawImage(this.staticcanvas.canvas, 0, 0);
	}

	private drawSector(sector: Point[], end: Point, color: string) {
		this.staticcanvas.context.strokeStyle = color;
		this.staticcanvas.context.beginPath();
		for (let i = 0; i < sector.length; i++)
			this.staticcanvas.context.lineTo(...this.calcPoint(sector[i].x, sector[i].y));
		this.staticcanvas.context.lineTo(...this.calcPoint(end.x, end.y));
		this.staticcanvas.context.stroke();
	}

	private drawChevrons(prev: Point, center: Point, next: Point) {
		this.staticcanvas.context.fillStyle = 'rgba(191, 191, 191, 0.7)';
		this.drawChevron(prev, center, next, 10);
		this.drawChevron(prev, center, next, -10);
	}

	private drawChevron(prev: Point, center: Point, next: Point, offset: number) {
		let width = 10, height = 10;
		let baseangle = Math.atan2(next.y - prev.y, next.x - prev.x);
		let angle = baseangle + Math.PI / 2;
		let point = { x: this.calcX(center.x), y: this.calcY(center.y) };
		point.x += Math.cos(baseangle) * offset;
		point.y += Math.sin(baseangle) * offset;
		this.staticcanvas.context.save();
		this.staticcanvas.context.translate(point.x, point.y);
		this.staticcanvas.context.rotate(angle);
		this.staticcanvas.context.translate(-1 * point.x, -1 * point.y);
		this.staticcanvas.context.beginPath();
		this.staticcanvas.context.lineTo(point.x - width / 2, point.y + height / 2);
		this.staticcanvas.context.lineTo(point.x - width / 2, point.y);
		this.staticcanvas.context.lineTo(point.x, point.y - height / 2);
		this.staticcanvas.context.lineTo(point.x + width / 2, point.y);
		this.staticcanvas.context.lineTo(point.x + width / 2, point.y + height / 2);
		this.staticcanvas.context.lineTo(point.x, point.y);
		this.staticcanvas.context.fill();
		this.staticcanvas.context.restore();
	}

	private drawSectorMarker(prev: Point, center: Point, next: Point, primary: string, secondary?: string) {
		let angle = Math.atan2(next.y - prev.y, next.x - prev.x);
		let point = { x: this.calcX(center.x), y: this.calcY(center.y) };
		let length = 30, width = 6;
		let xoffset = width / 2, yoffset = length / 2;
		this.staticcanvas.context.save();
		this.staticcanvas.context.translate(point.x, point.y);
		this.staticcanvas.context.rotate(angle);
		this.staticcanvas.context.translate(-1 * point.x, -1 * point.y);
		this.staticcanvas.context.fillStyle = primary;
		this.staticcanvas.context.fillRect(point.x - xoffset, point.y - yoffset, width, length);
		if (secondary) {
			let rows = 2;
			let cols = length / width * rows, size = width / rows;
			this.staticcanvas.context.fillStyle = secondary;
			for (let i = 0; i < cols; i++)
				this.staticcanvas.context.fillRect(point.x - xoffset + (i % 2 == 0 ? size : 0), point.y - yoffset + (i * size), size, size);
		}
		this.staticcanvas.context.restore();
	}

	private calcPoint(x: number, y: number): [number, number] {
		return [this.calcX(x), this.calcY(y)];
	}

	private calcX(x: number) {
		return x * this.scalefactor + this.canvas.width / 2;
	}

	private calcY(y: number) {
		return y * this.scalefactor + this.canvas.height / 2;
	}

	private calcScaleFactor(dx: number, dy: number) {
		let multiplier = .8;
		if (dx / dy > this.canvas.width / this.canvas.height)
			this.scalefactor = this.canvas.width / dx * multiplier;
		else
			this.scalefactor = this.canvas.height / dy * multiplier;
		let wx = 0, wy = 0;
		if (this.canvas.width > this.canvas.height)
			wx = this.canvas.width / 4;
		else
			wy = this.canvas.height / 2;
		this.scaled.dx = this.scalefactor * this.dx;
		this.scaled.dy = this.scalefactor * this.dy;
		this.scaled.cx = this.scalefactor * this.cx;
		this.scaled.cy = this.scalefactor * this.cy;
	}
}
