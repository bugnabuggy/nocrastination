import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { TaskContract } from '../../contracts/task-contract';
import { SchedulerService } from '../../services';

@Component({
	selector: 'app-task-checker',
	templateUrl: './task-checker.component.html',
	styleUrls: ['./task-checker.component.css']
})
export class TaskCheckerComponent implements OnInit {
	@Input() task: TaskContract;
	@Output() save: EventEmitter<any> = new EventEmitter();

	constructor(
		private shedulerSvc: SchedulerService
	) { }

	ngOnInit() {
	}

	onSave() {
		this.save.emit(this.task);
	}

	timespanMinutes(): number {
		return Math.round(((this.task.endDate as any) - (this.task.startDate as any)) / 60000);
	}

	duration(): string {
		const span = this.timespanMinutes();
		let str = '';
		str += Math.round(span / 1440) > 0 ? `${Math.round(span / 1440)} days ` : '';
		str += Math.round(span / 60) > 0 ? `${Math.round(span / 60)} hours ` : '';
		str += span % 60 > 0 ? `${span % 60} minutes ` : '';
		return '( ' + str + ' )';
	}

	calculatePoints(): string {
		let points = '';
		if (this.task.isFinished) {
			points = '+ ' + (this.timespanMinutes() * 2).toString() + ' Points';
		}

		return points;
	}
}
