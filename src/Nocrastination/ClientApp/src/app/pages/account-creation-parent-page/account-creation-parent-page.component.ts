import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ParentRegistration } from '../../contracts/parent-registreation';
import { RegistrationService, NotificationService, SecurityService, UserService } from '../../services';

@Component({
	selector: 'app-account-creation-parent-page',
	templateUrl: './account-creation-parent-page.component.html'
})
export class AccountCreationParentPagecomponentComponent {
	inProgress:boolean = false;
	parent: ParentRegistration = {
	};

	constructor(
		public router: Router,
		private regSvc: RegistrationService,
		private notifications: NotificationService,
		private securitySvc: SecurityService,
		private userSvc: UserService
	) {

	}

	createAccount() {
		if (!this.inProgress &&
			this.parent.fullName &&
			this.parent.userName &&
			this.parent.email &&
			this.parent.password.length > 3 &&
			this.parent.password === this.parent.confirmPassword) {
				this.inProgress = true;
			this.regSvc.registerParent(this.parent)
				.subscribe((val: any) => {
					this.securitySvc.setTokens(val.data[0]);
					this.userSvc.login(this.parent.userName, this.parent.password)
						.subscribe(
							(val: any) => {
								this.router.navigate(['/create-child-account']);
								this.inProgress = false;
							}
						);

				},
					err => {
						this.notifications.error('Registration error!');
						this.inProgress = false;
					});



		} else {
			this.notifications.error('Fill all form fields with right values');
		}

	}
}
