import { Component, OnInit } from '@angular/core';
import { SecurityService, UserService } from '../../services';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';

@Component({
	selector: 'app-parent-dashboard',
	templateUrl: './parent-dashboard.component.html',
	styleUrls: ['./parent-dashboard.component.css']
})
export class ParentDashboardComponent implements OnInit {

	constructor(
		private userSvc: UserService,
		private router: Router,
		private titleService: Title
	) { }

	ngOnInit() {
		this.titleService.setTitle('Nocrastination - Parent page');
	}

	logout() {
		this.userSvc.logout();
		this.router.navigate(['/']);
	}
}
