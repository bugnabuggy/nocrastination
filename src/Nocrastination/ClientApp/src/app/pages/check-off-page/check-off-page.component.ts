import { Component, OnInit } from '@angular/core';
import { TaskContract } from '../../contracts/task-contract';
import { SchedulerService, UserService } from '../../services';
import { ChildRegistration } from '../../contracts/child-registration';

@Component({
	selector: 'app-check-off-page',
	templateUrl: './check-off-page.component.html',
	styleUrls: ['./check-off-page.component.css']
})
export class CheckOffPageComponent implements OnInit {
	tasks: TaskContract[] = [];
	childName = '';
	childGender = '';

	constructor(
		private schedulerSvc: SchedulerService,
		private userSvc: UserService
	) { }

	ngOnInit() {
		this.schedulerSvc.getTasks()
			.subscribe(val => {
				this.tasks = val;
			});

		this.userSvc.getChild().
			subscribe((val: ChildRegistration) => {
				this.childName = val.fullName;
				this.childGender = val.sex === 'F' ? 'she' : 'he';
			});
	}

	save(task: TaskContract) {
		this.schedulerSvc.save(task).subscribe(
			val => { }
		);
	}

}
