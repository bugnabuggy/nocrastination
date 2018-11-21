import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginSelectPageComponent } from './login-select-page.component';

describe('LoginSelectPageComponent', () => {
  let component: LoginSelectPageComponent;
  let fixture: ComponentFixture<LoginSelectPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LoginSelectPageComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginSelectPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
