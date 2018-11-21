import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChildDashboardComponent } from './child-dashboard.component';

describe('ChildDashboardComponent', () => {
  let component: ChildDashboardComponent;
  let fixture: ComponentFixture<ChildDashboardComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChildDashboardComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChildDashboardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
