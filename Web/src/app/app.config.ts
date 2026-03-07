import { ApplicationConfig, ErrorHandler, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';
import { environment } from '../environments/environment';
import { routes } from './app.routes';
import { GlobalErrorHandler } from './global-error-handler';
import { HttpServerApiService, ServerApiServiceToken, StaticServerApiService } from './server-api.service/server-api.service';
import { HttpServerLiveService, ServerLiveServiceToken, StaticServerLiveService } from './server-live.service/server-live.service';

export const appConfig: ApplicationConfig = {
	providers: [
		provideBrowserGlobalErrorListeners(),
		provideRouter(routes),
		{ provide: ErrorHandler, useClass: GlobalErrorHandler },
		{ provide: ServerApiServiceToken, useClass: environment.static ? StaticServerApiService : HttpServerApiService },
		{ provide: ServerLiveServiceToken, useClass: environment.static ? StaticServerLiveService : HttpServerLiveService },
	]
};
