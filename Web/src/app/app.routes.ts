import { Routes } from '@angular/router';
import { Sessions } from './sessions/sessions';
import { Session } from './session/session';
import { Timing } from './session/timing/timing';
import { Laps } from './session/laps/laps';
import { EntryList } from './session/entry-list/entry-list';
import { TrackMap } from './session/track-map/track-map';
import { Chat } from './session/chat/chat';
import { BestLaps } from './best-laps/best-laps';
import { Settings } from './settings/settings';
import { About } from './about/about';
import { NotFound } from './not-found/not-found';

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
