import { Routes } from '@angular/router';
import { Sessions } from './sessions/sessions';
import { Session } from './session/session';
import { Timing } from './session/timing/timing';
import { History } from './session/history/history';
import { Laps } from './session/laps/laps';
import { EntryList } from './session/entry-list/entry-list';
import { TrackMap } from './session/track-map/track-map';
import { BestLaps } from './best-laps/best-laps';
import { Settings } from './settings/settings';
import { About } from './about/about';
import { NotFound } from './not-found/not-found';

export const routes: Routes = [
	{
		path: '',
		component: Sessions,
	},
	{
		path: 'Session/:sessionId',
		component: Session,
	},
	{
		path: 'Session/:sessionId/Timing',
		component: Timing,
	},
	{
		path: 'Session/:sessionId/History',
		component: History,
	},
	{
		path: 'Session/:sessionId/Laps/:carId',
		component: Laps,
	},
	{
		path: 'Session/:sessionId/History/Laps/:carId',
		component: Laps,
	},
	{
		path: 'Session/:sessionId/EntryList',
		component: EntryList,
	},
	{
		path: 'Session/:sessionId/TrackMap',
		component: TrackMap,
	},
	{
		path: 'BestLaps',
		component: BestLaps,
	},
	{
		path: 'Settings',
		component: Settings,
	},
	{
		path: 'About',
		component: About,
	},
	{
		path: '**',
		component: NotFound,
	},
];
