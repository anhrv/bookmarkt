import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IzdavacDetaljiComponent } from './izdavac-detalji.component';

describe('IzdavacDetaljiComponent', () => {
  let component: IzdavacDetaljiComponent;
  let fixture: ComponentFixture<IzdavacDetaljiComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IzdavacDetaljiComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IzdavacDetaljiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
