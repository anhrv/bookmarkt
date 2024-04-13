import { ComponentFixture, TestBed } from '@angular/core/testing';

import { IzdavacDodajComponent } from './izdavac-dodaj.component';

describe('IzdavacDodajComponent', () => {
  let component: IzdavacDodajComponent;
  let fixture: ComponentFixture<IzdavacDodajComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ IzdavacDodajComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(IzdavacDodajComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
