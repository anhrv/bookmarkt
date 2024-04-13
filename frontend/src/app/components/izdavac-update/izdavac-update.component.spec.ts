import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IzdavacUpdateComponent } from './izdavac-update.component';

describe('IzdavacUpdateComponent', () => {
  let component: IzdavacUpdateComponent;
  let fixture: ComponentFixture<IzdavacUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IzdavacUpdateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IzdavacUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
