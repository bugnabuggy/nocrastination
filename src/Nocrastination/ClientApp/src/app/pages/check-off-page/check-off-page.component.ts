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
	totalPoints: number;
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
				this.calculatePoints();
			});

		this.userSvc.getChild().
			subscribe((val: ChildRegistration) => {
				this.childName = val.fullName;
				this.childGender = val.sex === 'F' ? 'she' : 'he';
			});
	}

	save(task: TaskContract) {
		task.isFinished = !task.isFinished;
		this.schedulerSvc.save(task)
			.subscribe(val => {
				this.calculatePoints();
				console.log(`Task ${task.name} marked as ${task.isFinished ? 'finished' : 'not finished'}`)
			});
	}

	calculatePoints() {
		this.totalPoints = 0;
		this.tasks.forEach(x => {
			if (!x.isFinished) {
				return;
			}
			const duration = Math.round(((x.endDate as any) - (x.startDate as any)) / 60000);
			this.totalPoints += duration * 2;
		});
	}

}
