import { Component, OnInit } from '@angular/core';
import { SecurityService, UserService } from '../../services';
import { Router } from '@angular/router';

@Component({
	selector: 'app-parent-dashboard',
	templateUrl: './parent-dashboard.component.html',
	styleUrls: ['./parent-dashboard.component.css']
})
export class ParentDashboardComponent implements OnInit {

	constructor(
		private userSvc: UserService,
		private router: Router
	) { }

	ngOnInit() {
	}

	logout() {
		this.userSvc.logout();
		this.router.navigate(['/']);
	}
}
