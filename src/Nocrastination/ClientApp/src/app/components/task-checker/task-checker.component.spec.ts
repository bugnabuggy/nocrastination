import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskCheckerComponent } from './task-checker.component';

describe('TaskCheckerComponent', () => {
  let component: TaskCheckerComponent;
  let fixture: ComponentFixture<TaskCheckerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TaskCheckerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskCheckerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
