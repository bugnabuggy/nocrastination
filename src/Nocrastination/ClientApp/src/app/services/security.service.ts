import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Router, CanActivate } from '@angular/router';
import { share, flatMap, map } from 'rxjs/operators';
import { } from 'rxjs';
import { Endpoints } from '../enums/Endpoints';



@Injectable()
export class SecurityService {
	isActivate: boolean = true;
	token: string;

	isAdmin: boolean = false;
	tokenExpirationDate: Date;

	constructor(
		public router: Router,
		public http: HttpClient
	) {
		const userTokens = JSON.parse(localStorage.getItem('userTokens'));
		if (userTokens) {
			this.setTokens(userTokens);
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

	getToken(login, password) {
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
			httpOptions)
			.pipe(
				share(),
			);

		observable
			.subscribe(
				val => {
					this.setTokens(val);
				}
			);

		return observable;
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

	isAuthenticated() {
		return !!this.token;
	}
}
