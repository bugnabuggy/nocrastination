import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddToSchedulePageComponent } from './add-to-schedule-page.component';

describe('AddToSchedulePageComponent', () => {
  let component: AddToSchedulePageComponent;
  let fixture: ComponentFixture<AddToSchedulePageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddToSchedulePageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddToSchedulePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
