import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AutorDetaljiComponent } from './autor-detalji.component';

describe('AutorDetaljiComponent', () => {
  let component: AutorDetaljiComponent;
  let fixture: ComponentFixture<AutorDetaljiComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AutorDetaljiComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AutorDetaljiComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
