import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { switchMap, flatMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { LoginContract } from '../../contracts/login-contract';
import { SecurityService, NotificationService, UserService } from '../../services';

@Component({
	selector: 'app-log-in',
	templateUrl: './log-in-page.component.html'
})
export class LogInPageComponent implements OnInit {
	type: string = 'parent';
	message?: string = '';
	data: LoginContract = {};

	constructor(
		private route: ActivatedRoute,
		private router: Router,
		private securitySvc: SecurityService,
		private userSvc: UserService,
		private notification: NotificationService
	) { }

	ngOnInit() {
		this.route.params.subscribe(
			(params: any) => {
				this.type = params.type;
				console.log(`login type = ${this.type}`);
				this.message = this.type == 'child'
					? 'Let`s see your progress!'
					: 'Connect with your child!';
				return of();
			});
	}

	login(form) {
		if (form.valid) {
			this.userSvc.login(this.data.username, this.data.password)
				.pipe(
					flatMap(x => {
						return this.userSvc.getUser();
					})
				)
				.subscribe(
					val => {
						if (this.type == 'child') {
							this.router.navigate(['/child-dashboard']);
						} else {
							this.router.navigate(['/parent-dashboard']);
						}
					},
					err => {
						this.notification.error('Error on login');
					}
				);
		}
	}
}
