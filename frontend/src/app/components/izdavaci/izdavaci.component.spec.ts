import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IzdavaciComponent } from './izdavaci.component';

describe('IzdavaciComponent', () => {
  let component: IzdavaciComponent;
  let fixture: ComponentFixture<IzdavaciComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IzdavaciComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IzdavaciComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
