import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginTwofactorComponent } from './login-twofactor.component';

describe('LoginTwofactorComponent', () => {
  let component: LoginTwofactorComponent;
  let fixture: ComponentFixture<LoginTwofactorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoginTwofactorComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoginTwofactorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
