import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AutorDodajComponent } from './autor-dodaj.component';

describe('AutorDodajComponent', () => {
  let component: AutorDodajComponent;
  let fixture: ComponentFixture<AutorDodajComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AutorDodajComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AutorDodajComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
