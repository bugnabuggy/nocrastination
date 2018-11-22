import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Endpoints } from '../enums/Endpoints';
import { Observable, of } from 'rxjs';
import { TaskContract } from '../contracts/task-contract';
import { map } from 'rxjs/operators';


@Injectable()
export class SchedulerService {

	constructor(
		private http: HttpClient
	) { }


	getLates(): Observable<TaskContract> {
		return this.http.get(Endpoints.api.latestTask)
			.pipe(
				map((x: any) => {
					if (x.data.length < 1 ) {
						return { name: 'You do not have any upcoming tasks, ask your parents add them.'} as TaskContract;
					}
					return x.data[0];
				})
			);
	}


	getTasks(): Observable<TaskContract[]> {
		return this.http.get<TaskContract[]>(Endpoints.api.task)
			.pipe(
				map((x: any) => {
					x.data = (x.data as TaskContract[]).map(y => {
						const endDate = new Date(y.endDate as any);
						endDate.setTime(endDate.getTime() - new Date().getTimezoneOffset() * 60000);
						y.endDate = endDate;

						const startDate = new Date(y.startDate as any);
						startDate.setTime(startDate.getTime() - new Date().getTimezoneOffset() * 60000);
						y.startDate = startDate;
						return y;
					});
					return x.data;
				})
			);
	}

	save(task: TaskContract): Observable<any> {
		let observable: Observable<any>;

		if (!task.id) {
			observable = this.http.post(
				Endpoints.api.task,
				task
			);
		} else {
			observable = this.http.put(
				Endpoints.api.task + '/' + task.id,
				task
			);
		}

		return observable.pipe(map((x: any) => x.data[0]));
	}


	delete(task: TaskContract): Observable<any> {
		if (task.id) {
			return this.http.delete(Endpoints.api.task + '/' + task.id);
		} else {
			return of(null);
		}

	}
}
