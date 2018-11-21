
import { Injectable, ApplicationRef } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpResponse,
  HttpErrorResponse
} from '@angular/common/http';
import { SecurityService,
         UserService } from '../services';
         import { Observable, throwError  } from 'rxjs';
         import { catchError, tap } from 'rxjs/operators';
import { Router } from '@angular/router';

@Injectable()
export class NocrastinationHttpInterceptor implements HttpInterceptor {
  updateInProgress: boolean = false;
  waitingRequests: HttpRequest<any>[] = [];

  constructor(public securitySvc: SecurityService,
    public router: Router,
    private userSvc: UserService
  ) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.userSvc.requestsInProgress++;
    if (this.securitySvc.token) {
      request = request.clone({
        setHeaders: {
          Authorization: `Bearer ${this.securitySvc.getToken()}`
        }
      });
    }
    return next.handle(request).pipe(tap((event: HttpEvent<any>) => {
      if (event instanceof HttpResponse) {
        this.userSvc.requestsInProgress--;
        // do stuff with response if you want
      }
    }), (err: any) => {
      this.userSvc.requestsInProgress--;
      if (err instanceof HttpErrorResponse) {
        if (err.status === 401) {
          this.securitySvc.clearTokens();
          this.router.navigate(['authorization']);
        }
      }
      return err;
    });

  }
}
