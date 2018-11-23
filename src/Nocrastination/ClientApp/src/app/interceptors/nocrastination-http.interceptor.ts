
import { Injectable, ApplicationRef } from '@angular/core';
import {
	HttpRequest,
	HttpHandler,
	HttpEvent,
	HttpInterceptor,
	HttpResponse,
	HttpErrorResponse
} from '@angular/common/http';
import {
	SecurityService,
	UserService
} from '../services';
import { Observable, throwError } from 'rxjs';
import { catchError, tap, share } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class NocrastinationHttpInterceptor implements HttpInterceptor {
	waitingRequests: HttpRequest<any>[] = [];

	constructor(public securitySvc: SecurityService,
		public router: Router,
		private userSvc: UserService
	) { }

	intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
		// somethin is wrong if i do that ↓
		this.userSvc.incRequest();

		if (this.securitySvc.token) {
			request = request.clone({
				setHeaders: {
					Authorization: `Bearer ${this.securitySvc.token}`
				}
			});
		}

		return next.handle(request).pipe(
			share(),
			tap((event: HttpEvent<any>) => {
				if (event instanceof HttpResponse) {
					this.userSvc.decRequest();
					// do stuff with response if you want
				}
			}, (err: any) => {
				this.userSvc.decRequest();
				if (err instanceof HttpErrorResponse) {
					if (err.status === 401) {
						this.securitySvc.clearTokens();
						this.router.navigate(['authorization']);
					}
				}
				return err;
			}));

	}
}
