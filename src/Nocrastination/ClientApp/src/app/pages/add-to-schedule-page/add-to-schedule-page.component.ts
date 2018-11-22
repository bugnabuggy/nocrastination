import { Component, OnInit } from '@angular/core';
import { SchedulerService, NotificationService } from '../../services';
import { TaskContract } from '../../contracts/task-contract';

@Component({
  selector: 'app-add-to-schedule-page',
  templateUrl: './add-to-schedule-page.component.html',
  styleUrls: ['./add-to-schedule-page.component.css']
})
export class AddToSchedulePageComponent implements OnInit {
  tasks: TaskContract[] = [];

  constructor(
    private schedulerSvc: SchedulerService,
    private notification: NotificationService
  ) { }

  ngOnInit() {
    this.schedulerSvc.getTasks()
      .subscribe(
        (values: TaskContract[]) => {
          this.tasks = values;
        },
        err => {
          this.notification.error('Error getting tasks');
        }
      );
  }

  addTask() {
    this.tasks.push({} as TaskContract);
  }

  taskChanged(task: TaskContract) {
    if (task.name
      && task.startDate
      && task.endDate
    ) {
      this.schedulerSvc.save(task)
        .subscribe(
          val => {
            const index = this.tasks.findIndex(x => x === task);
            this.tasks[index] = val;
            console.log('task chaged ' + task.name);
          },
          err => {
            this.notification.error('Error saving task');
          }
        );
    }
  }

  deleteTask(task: TaskContract) {
    this.schedulerSvc.delete(task)
      .subscribe(
        val => {
          const index = this.tasks.findIndex(x => x === task);
          this.tasks.splice(index, 1);
        }
      );
  }
}
