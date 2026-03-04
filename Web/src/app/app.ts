import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterOutlet, UrlSegment } from '@angular/router';
import { Header } from './header/header';
import { filter, map } from 'rxjs/operators';

type PathMatcher = (x: string) => boolean;
type NullablePathMatcher = PathMatcher | null;
type RouteMatcher = { path: NullablePathMatcher[] };

@Component({
	selector: 'app-root',
	imports: [RouterOutlet, Header],
	templateUrl: './app.html',
	styleUrl: './app.css'
})
export class App {
	private static readonly fluidRoutes: RouteMatcher[] = [
		{ path: [(x: string) => x == 'Session', null, (x: string) => x == 'Timing'] }
	];
	private readonly router = inject(Router);
	private readonly route = inject(ActivatedRoute);
	protected readonly title = signal('Web');
	protected readonly container = signal('container');

	constructor() {
		this.router.events.pipe(
			filter(e => e instanceof NavigationEnd),
			map(() => this.rootRoute(this.route)),
			filter((route: ActivatedRoute) => route.outlet === 'primary'),
		).subscribe((route: ActivatedRoute) => {
			let isFluid = this.container() == 'container-fluid';
			if(isFluid != App.isFluid(route.snapshot.url))
				this.container.set(isFluid ? 'container' : 'container-fluid');
		});
	}

	private rootRoute(route: ActivatedRoute): ActivatedRoute {
		while (route.firstChild) {
			route = route.firstChild;
		}
		return route;
	}

	private static isFluid(url: UrlSegment[]) {
		for (let link of this.fluidRoutes) {
			let match = url.length == link.path.length;
			for (let i = 0; i < url.length; i++) {
				let matcher = link.path[i];
				if (i < link.path.length && matcher)
					match = match && matcher(url[i].path);
			}
			if(match)
				return true;
		}
		return false;
	}
}
