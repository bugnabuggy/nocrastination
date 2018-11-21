import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Router, CanActivate } from '@angular/router';
import { share } from 'rxjs/operators';
import { Endpoints } from '../enums/Endpoints';



@Injectable()
export class SecurityService {
    isActivate: boolean = true;
    token: string ;
    userId: string;
    isAdmin: boolean = false;
    tokenExpirationDate: Date;

    constructor(
        public router: Router,
        public http: HttpClient
    ) {
        console.log('security serviec constructor');
        const userTokens = JSON.parse(localStorage.getItem('userTokens'));
        if (userTokens) {
            this.setTokens(userTokens);
        }

        const userId = JSON.parse(localStorage.getItem('userId'));
        if (userId) {
            this.userId = userId;
        }
    }

    canActivate(): boolean {
        if (!this.isAuthenticated()) {
          this.router.navigate(['authorization']);
          this.isAdmin = false;
          this.isActivate = false;
          return false;
        } else {
            this.isAdmin = true;
          this.isActivate = true;
          return true;
        }
      }

    login(login, password) {
        const httpOptions = {
            headers: new HttpHeaders({
                'Content-Type': 'application/x-www-form-urlencoded',
            })
        };
        const body = new HttpParams()
            .set('client_id', 'mvc')
            .set('client_secret', 'secret')
            .set('grant_type', 'password')
            .set('username', login)
            .set('password', password);

        const url = Endpoints.api.authorization.login;

        const observable = this.http.post(url,
            body,
            httpOptions).pipe(share());

        return observable;
    }

    setUser(user: any) {
        this.userId = user.id;
        localStorage.setItem('userId', JSON.stringify(user.id));
    }

    setTokens(authResponse: any): any {
        this.token = authResponse.access_token;

        if (authResponse.tokenExpirationDate) {
            this.tokenExpirationDate = authResponse.tokenExpirationDate;
        } else {
            this.tokenExpirationDate = new Date();
            this.tokenExpirationDate.setSeconds(authResponse.expires_in);
            authResponse.tokenExpirationDate = this.tokenExpirationDate;
            delete authResponse['expires_in'];
        }

        localStorage.setItem('userTokens', JSON.stringify(authResponse));
    }

    clearTokens() {
        this.token = null;
        this.tokenExpirationDate = null;
        localStorage.removeItem('userTokens');
    }

    getToken(): any {
        return this.token;
    }

    isAuthenticated() {
        return !!this.token;
    }
}
