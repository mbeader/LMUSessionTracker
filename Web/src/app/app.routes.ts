import { Routes } from '@angular/router';
import { Sessions } from './sessions/sessions';
import { Session } from './session/session';
import { History } from './session/history/history';
import { Laps } from './session/laps/laps';
import { EntryList } from './session/entry-list/entry-list';
import { BestLaps } from './best-laps/best-laps';

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
		path: 'BestLaps',
		component: BestLaps,
	},
];
