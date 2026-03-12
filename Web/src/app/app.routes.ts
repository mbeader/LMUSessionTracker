import { Routes } from '@angular/router';
import { Sessions } from './sessions/sessions/sessions.component';
import { Session } from './sessions/session/session.component';
import { Timing } from './sessions/timing/timing.component';
import { Laps } from './sessions/car-laps/car-laps.component';
import { EntryList } from './sessions/entry-list/entry-list.component';
import { TrackMap } from './sessions/track-map/track-map.component';
import { Chat } from './sessions/chat/chat.component';
import { BestLaps } from './best-laps/best-laps/best-laps.component';
import { Settings } from './settings/settings/settings.component';
import { About } from './about/about.component';
import { NotFound } from './not-found/not-found.component';

const title = function(t: string) { return `${t} - LMUSessionTracker`; };

export const routes: Routes = [
	{
		path: '',
		component: Sessions,
		title: title('Sessions')
	},
	{
		path: 'Session/:sessionId',
		component: Session,
		title: title('Session')
	},
	{
		path: 'Session/:sessionId/Timing',
		component: Timing,
		title: title('Timing')
	},
	{
		path: 'Session/:sessionId/Laps/:carId',
		component: Laps,
		title: title('Laps')
	},
	{
		path: 'Session/:sessionId/EntryList',
		component: EntryList,
		title: title('Entry list')
	},
	{
		path: 'Session/:sessionId/TrackMap',
		component: TrackMap,
		title: title('Track map')
	},
	{
		path: 'Session/:sessionId/Chat',
		component: Chat,
		title: title('Chat')
	},
	{
		path: 'BestLaps',
		component: BestLaps,
		title: title('Best laps')
	},
	{
		path: 'Settings',
		component: Settings,
		title: title('Settings')
	},
	{
		path: 'About',
		component: About,
		title: title('About')
	},
	{
		path: '**',
		component: NotFound,
		title: title('Not found')
	},
];
