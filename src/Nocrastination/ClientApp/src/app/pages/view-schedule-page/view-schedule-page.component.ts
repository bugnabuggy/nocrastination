import { Component, OnInit } from '@angular/core';

import {
	startOfDay,
	endOfDay,
	subDays,
	addDays,
	endOfMonth,
	isSameDay,
	isSameMonth,
	addHours
} from 'date-fns';
import { Subject } from 'rxjs';

import {
	CalendarEvent,
	CalendarEventAction,
	CalendarEventTimesChangedEvent,
	CalendarView
} from 'angular-calendar';
import { SchedulerService, UserService } from '../../services';
import { TaskContract } from '../../contracts/task-contract';
import { Router } from '@angular/router';

const colors: any = {
	red: {
		primary: '#ad2121',
		secondary: '#FAE3E3'
	},
	blue: {
		primary: '#1e90ff',
		secondary: '#D1E8FF'
	},
	yellow: {
		primary: '#e3bc08',
		secondary: '#FDF1BA'
	}
};

@Component({
	selector: 'app-view-schedule-page',
	templateUrl: './view-schedule-page.component.html',
	styleUrls: ['./view-schedule-page.component.css']
})
export class ViewSchedulePageComponent implements OnInit {
	viewDate: Date = new Date();

	events: CalendarEvent[] = [];

	tasks: TaskContract[];
	startHour = 7;
	endHour = 21;

	constructor(
		private schedulerSvc: SchedulerService,
		private userSvc: UserService,
		private router: Router
	) { }

	ngOnInit() {
		this.schedulerSvc.getTasks()
			.subscribe(val => {
				this.startHour = this.getStart(val);
				this.endHour = this.getEnd(val);
				this.events = this.mapToEvents(val);
			});
	}

	getStart(tasks: TaskContract[]): number {
		let min = 23;
		tasks.forEach(x => {
			min = x.startDate.getHours() < min
			? x.startDate.getHours()
			: min;
		});
		return min;
	}

	getEnd(tasks: TaskContract[]): number {
		let max = 1;
		tasks.forEach(x => {
			max = x.startDate.getHours() > max
			? x.startDate.getHours()
			: max;
		});
		return max;
	}

	mapToEvents(tasks: TaskContract[]): CalendarEvent[] {
		const events = tasks.map(x => {
			return {
				start: x.startDate,
				end: x.endDate,
				title: x.name,
				color: colors.yellow,
			} as CalendarEvent;
		});
		return events;
	}

	goToDashboard() {
		if (this.userSvc.isChild) {
			this.router.navigate(['/child-dashboard']);
		} else {
			this.router.navigate(['/parent-dashboard']);
		}
	}
}
