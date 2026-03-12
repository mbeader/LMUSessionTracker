import { Routes } from '@angular/router';
import { SessionsComponent } from './sessions/sessions/sessions.component';
import { SessionComponent } from './sessions/session/session.component';
import { TimingComponent } from './sessions/timing/timing.component';
import { CarLapsComponent } from './sessions/car-laps/car-laps.component';
import { EntryListComponent } from './sessions/entry-list/entry-list.component';
import { TrackMapComponent } from './sessions/track-map/track-map.component';
import { ChatComponent } from './sessions/chat/chat.component';
import { BestLapsComponent } from './best-laps/best-laps/best-laps.component';
import { SettingsComponent } from './settings/settings/settings.component';
import { AboutComponent } from './about/about.component';
import { NotFoundComponent } from './not-found/not-found.component';

const title = function(t: string) { return `${t} - LMUSessionTracker`; };

export const routes: Routes = [
	{
		path: '',
		component: SessionsComponent,
		title: title('Sessions')
	},
	{
		path: 'Session/:sessionId',
		component: SessionComponent,
		title: title('Session')
	},
	{
		path: 'Session/:sessionId/Timing',
		component: TimingComponent,
		title: title('Timing')
	},
	{
		path: 'Session/:sessionId/Laps/:carId',
		component: CarLapsComponent,
		title: title('Laps')
	},
	{
		path: 'Session/:sessionId/EntryList',
		component: EntryListComponent,
		title: title('Entry list')
	},
	{
		path: 'Session/:sessionId/TrackMap',
		component: TrackMapComponent,
		title: title('Track map')
	},
	{
		path: 'Session/:sessionId/Chat',
		component: ChatComponent,
		title: title('Chat')
	},
	{
		path: 'BestLaps',
		component: BestLapsComponent,
		title: title('Best laps')
	},
	{
		path: 'Settings',
		component: SettingsComponent,
		title: title('Settings')
	},
	{
		path: 'About',
		component: AboutComponent,
		title: title('About')
	},
	{
		path: '**',
		component: NotFoundComponent,
		title: title('Not found')
	},
];
