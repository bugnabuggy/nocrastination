import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CheckOffPageComponent } from './check-off-page.component';

describe('CheckOffPageComponent', () => {
  let component: CheckOffPageComponent;
  let fixture: ComponentFixture<CheckOffPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CheckOffPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CheckOffPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
