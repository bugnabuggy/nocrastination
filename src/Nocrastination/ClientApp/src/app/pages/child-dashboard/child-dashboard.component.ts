import { Component, OnInit } from '@angular/core';
import { SecurityService, UserService, SchedulerService } from '../../services';
import { Router } from '@angular/router';
import { TaskContract } from '../../contracts/task-contract';

@Component({
	selector: 'app-child-dashboard',
	templateUrl: './child-dashboard.component.html',
	styleUrls: ['./child-dashboard.component.css']
})
export class ChildDashboardComponent implements OnInit {
	latestTask: TaskContract = {name: ''};
	name: string = '';

	constructor(
		public userSvc: UserService,
		private router: Router,
		private schedulerSvc: SchedulerService
	) { }

	ngOnInit() {
		this.schedulerSvc.getLates()
		.subscribe((val: TaskContract) => {
			this.latestTask = val;
		});
	}

	logout() {
		this.userSvc.logout();
		this.router.navigate(['/']);
	}
}
