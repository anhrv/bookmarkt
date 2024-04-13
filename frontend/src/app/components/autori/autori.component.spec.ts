import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AutoriComponent } from './autori.component';

describe('AutoriComponent', () => {
  let component: AutoriComponent;
  let fixture: ComponentFixture<AutoriComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AutoriComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AutoriComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
