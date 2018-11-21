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
    if (!task.id) {
      return this.http.post(
        Endpoints.api.task,
        task
      );
    } else {
      return this.http.put(
        Endpoints.api.task + '/' + task.id,
        task
      );
    }
  }


  delete(task: TaskContract): Observable<any> {
    if (task.id) {
      return this.http.delete(Endpoints.api.task + '/' + task.id);
    } else {
      return of(null);
    }

  }
}
