import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { LoginContract } from '../../contracts/login-contract';
import { SecurityService, NotificationService } from '../../services';

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
    if ( form.valid) {
      this.securitySvc.login(this.data.username, this.data.password)
      .subscribe(
        val => {
          this.securitySvc.setTokens(val);
          if (this.type == 'child') {
            this.router.navigate(['/child-dashboard']);
          } else {
            this.router.navigate(['/create-child-account']);
           }
        },
        err => {
          this.notification.error('Error on login');
        }
      );
    }
  }
}
