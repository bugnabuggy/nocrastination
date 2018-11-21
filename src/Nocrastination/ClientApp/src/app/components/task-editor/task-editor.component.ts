import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { TaskContract } from '../../contracts/task-contract';

@Component({
  selector: 'app-task-editor',
  templateUrl: './task-editor.component.html',
  styleUrls: ['./task-editor.component.css']
})
export class TaskEditorComponent implements OnInit {
  @Input() task: TaskContract;
  @Output() save: EventEmitter<any> = new EventEmitter();
  @Output() delete: EventEmitter<any> = new EventEmitter();

  constructor() { }

  ngOnInit() {
  }

  onSave() {
    this.save.emit(this.task);
  }

  onDelete() {
    this.delete.emit(this.task.id);
  }

}
