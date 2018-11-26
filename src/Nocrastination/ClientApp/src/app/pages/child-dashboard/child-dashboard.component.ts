import { Component, OnInit } from '@angular/core';
import { SecurityService, UserService, SchedulerService } from '../../services';
import { Router } from '@angular/router';
import { TaskContract } from '../../contracts/task-contract';
import { ChildStatusContract } from '../../contracts/child-status-contract';

@Component({
	selector: 'app-child-dashboard',
	templateUrl: './child-dashboard.component.html',
	styleUrls: ['./child-dashboard.component.css']
})
export class ChildDashboardComponent implements OnInit {
	latestTask: TaskContract = {name: ''};
	name: string = '';
	avatar: string = '';

	status: ChildStatusContract = {score: 0, itemImageUrl: '', itemName: ''};

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

		this.userSvc.getStatus()
			.subscribe( val => {
				this.status = val;
				this.avatar = this.userSvc.gender === 'F'
				? '/assets/avaF.png'
				: '/assets/ava.png';
			});
	}

	logout() {
		this.userSvc.logout();
		this.router.navigate(['/']);
	}
}
 