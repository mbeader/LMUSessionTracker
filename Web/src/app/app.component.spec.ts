import { TestBed } from '@angular/core/testing';
import { ActivatedRoute } from '@angular/router';
import { AppComponent } from './app.component';

describe('AppComponent', () => {
	beforeEach(async () => {
		await TestBed.configureTestingModule({
			imports: [AppComponent],
			providers: [
				{ provide: ActivatedRoute, useValue: { snapshot: { paramMap: new Map() } } },
			]
		}).compileComponents();
	});

	it('should create the app', () => {
		const fixture = TestBed.createComponent(AppComponent);
		const app = fixture.componentInstance;
		expect(app).toBeTruthy();
	});

	it('should render title', async () => {
		const fixture = TestBed.createComponent(AppComponent);
		await fixture.whenStable();
		const compiled = fixture.nativeElement as HTMLElement;
		expect(compiled.querySelector('header a')?.textContent).toContain('LMUSessionTracker');
	});
});
