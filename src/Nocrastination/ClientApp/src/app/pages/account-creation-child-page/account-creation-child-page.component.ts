import { Component } from '@angular/core';
import { ChildRegistration } from '../../contracts/child-registration';
import { NotificationService, RegistrationService, SecurityService } from '../../services';
import { Router } from '@angular/router';

@Component({
  selector: 'app-account-creation-child-page',
  templateUrl: './account-creation-child-page.component.html'
})
export class AccountCreationChildPageComponent {
    child: ChildRegistration = {};
    labelPosition: any;

    constructor(
      private notification: NotificationService,
      private regSvc: RegistrationService,
      private router: Router,
      private securitySvc: SecurityService
    ) {}

    createAccount (form) {
      debugger;
      if (form.valid &&
          this.child.age > 0 &&
          this.child.password == this.child.confirmPassword) {
            this.child.parentId = this.securitySvc.userId;
            this.regSvc.registerChild(this.child)
            .subscribe(
              (val) => {
                this.router.navigate(['/parent-dashboard']);
              },
              err => {
                this.notification.error('Error registering child!');
              }
            );

      } else {
        this.notification.error('Fill all form fields with right values');
      }
      console.log(this.child);
    }
}
