import { ChangeDetectorRef, Component, inject } from '@angular/core';
import { NavigationEnd, Router, RouterLink, ActivatedRoute, UrlSegment } from '@angular/router';
import { filter, map } from 'rxjs/operators';

@Component({
	selector: 'app-header',
	imports: [RouterLink],
	templateUrl: './header.html',
	styleUrl: './header.css',
})
export class Header {
	private static readonly navLinks = [
		{
			id: 'home',
			path: []
		},
		{
			id: 'session',
			path: [(x: string) => x == 'Session'],
			links: [
				{
					id: 'live',
					path: [(x: string) => x == 'Session', null]
				},
				{
					id: 'history',
					path: [(x: string) => x == 'Session', null, (x: string) => x == 'History']
				}
			]
		},
		{
			id: 'best-laps',
			path: [(x: string) => x == 'BestLaps']
		}
	];
	private readonly ref = inject(ChangeDetectorRef);
	private readonly router = inject(Router);
	private readonly route = inject(ActivatedRoute);
	sessionId: string | null = null;
	navLink: string = '';
	navSubLink: string = '';

	constructor() {
		this.router.events.pipe(
			filter(e => e instanceof NavigationEnd),
			map(() => this.rootRoute(this.route)),
			filter((route: ActivatedRoute) => route.outlet === 'primary'),
		).subscribe((route: ActivatedRoute) => {
			let newSessionId = route.snapshot.paramMap.get('sessionId');
			let newNavLink = Header.findCurrentNavLink(route.snapshot.url);
			let navChanged = this.navLink != newNavLink.navLink || this.navSubLink != newNavLink.navSubLink;
			if (this.sessionId != newSessionId || navChanged) {
				this.sessionId = newSessionId;
				this.navLink = newNavLink.navLink;
				this.navSubLink = newNavLink.navSubLink;
				this.ref.markForCheck();
			}
		});
	}

	private rootRoute(route: ActivatedRoute): ActivatedRoute {
		while (route.firstChild) {
			route = route.firstChild;
		}
		return route;
	}

	static findCurrentNavLink(url: UrlSegment[]) {
		let rootLink = '', subLink = '';
		for (let link of this.navLinks) {
			let match = url.length == link.path.length || (link.links && url.length >= link.path.length);
			for (let i = 0; i < url.length; i++) {
				if (i < link.path.length && link.path[i])
					match = match && link.path[i](url[i].path);
			}
			if (match)
				rootLink = link.id;
			if (match && link.links) {
				for (let sublink of link.links) {
					let submatch = url.length >= sublink.path.length;
					for (let i = 0; i < url.length; i++) {
						if (i < sublink.path.length && sublink.path[i])
							submatch = submatch && (sublink.path[i]?.(url[i].path) ?? true);
					}
					if (submatch)
						subLink = sublink.id;
				}
			}
		}
		return { navLink: rootLink, navSubLink: subLink };
	}
}
