import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ParentRegistration } from '../../contracts/parent-registreation';
import { RegistrationService, NotificationService, SecurityService } from '../../services';

@Component({
    selector: 'app-account-creation-parent-page',
    templateUrl: './account-creation-parent-page.component.html'
})
export class AccountCreationParentPagecomponentComponent {
    parent: ParentRegistration = {
    };

    constructor(
        public router: Router,
        private regSvc: RegistrationService,
        private notifications: NotificationService,
        private securitySvc: SecurityService,
    ) {

    }

    createAccount() {
        if (this.parent.fullName &&
            this.parent.userName &&
            this.parent.email &&
            this.parent.password.length > 3 &&
            this.parent.password === this.parent.confirmPassword) {

            this.regSvc.registerParent(this.parent)
                .subscribe((val: any) => {
                    debugger;
                    this.securitySvc.setTokens(val.data[0]);
                    this.router.navigate(['/create-child-account']);
                },
                err => {
                    this.notifications.error('Registration error!');
                });



        } else {
            this.notifications.error('Fill all form fields with right values');
        }

    }
}
