import { Component, OnInit } from '@angular/core';
import { SecurityService } from '../../services';
import { Router } from '@angular/router';

@Component({
  selector: 'app-parent-dashboard',
  templateUrl: './parent-dashboard.component.html',
  styleUrls: ['./parent-dashboard.component.css']
})
export class ParentDashboardComponent implements OnInit {

  constructor(
    private securitySvc: SecurityService,
    private router: Router
  ) { }

  ngOnInit() {
  }

  logout() {
    this.securitySvc.clearTokens();
    this.router.navigate(['/']);
  }
}
