import { Injectable } from '@angular/core';
import { SecurityService } from './security.service';
import { HttpClient } from '@angular/common/http';
import { Endpoints } from '../enums/Endpoints';
import { Observable } from 'rxjs';
import { share, flatMap } from 'rxjs/operators';
import { ChildStatusContract } from '../contracts/child-status-contract';

@Injectable()
export class UserService {
	requestsInProgress: number = 0;
	pageTitle: string;
	userId: string;
	isChild: boolean;
	gender: string;
	currentImg: string;
	name: string;

	constructor(
		private securitySvc: SecurityService,
		private http: HttpClient
	) {
		const userId = JSON.parse(localStorage.getItem('userId'));
		if (userId) { this.userId = userId; }
		const isChild = JSON.parse(localStorage.getItem('isChild'));
		this.isChild = isChild;

		const name = JSON.parse(localStorage.getItem('fullname'));
		if (name) { this.name = name; }

		const gender = JSON.parse(localStorage.getItem('gender'));
		if (gender) { this.gender = gender; }
	}

	incRequest() {
		this.requestsInProgress++;
	}

	decRequest() {
		this.requestsInProgress--;
	}

	setUser(user: any) {
		this.userId = user.id;
		this.isChild = user.isChild;
		this.name = user.fullName;
		this.gender = user.sex;
		localStorage.setItem('userId', JSON.stringify(user.id));
		localStorage.setItem('isChild', JSON.stringify(user.isChild));
		localStorage.setItem('gender', JSON.stringify(user.sex));
		localStorage.setItem('fullname', JSON.stringify(user.fullName || ''));
	}

	login(login, password) {
		const observable = this.securitySvc.getToken(login, password)
		.pipe(
			share(),
			flatMap(x => {
				return this.getUser();
			})
		);

		return observable;
	}

	logout() {
		this.securitySvc.clearTokens();
		this.clearUser();
	}

	clearUser() {
		localStorage.removeItem('userId');
		localStorage.removeItem('isChild');
		localStorage.removeItem('fullname');
		localStorage.removeItem('gender');
	}

	canActivate(): boolean {
		return this.isChild === false;
	}

	getChild(): Observable<any> {
		return this.http.get(Endpoints.api.child);
	}

	getUser(): Observable<any> {
		const observable = this.http.get(Endpoints.api.user).pipe(share());

		observable.subscribe(val => {
			this.setUser(val);
		});

		return observable;
	}

	getStatus(): Observable<ChildStatusContract> {
		const observable =  this.http.get<ChildStatusContract>(Endpoints.api.childStatus).pipe(share());

		observable.subscribe(val => {
			this.currentImg  = val.itemImageUrl;
		});

		return observable;
	}
}
