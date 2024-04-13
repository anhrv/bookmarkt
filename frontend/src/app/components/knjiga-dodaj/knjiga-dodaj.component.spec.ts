import { ComponentFixture, TestBed } from '@angular/core/testing';

import { KnjigaDodajComponent } from './knjiga-dodaj.component';

describe('KnjigaDodajComponent', () => {
  let component: KnjigaDodajComponent;
  let fixture: ComponentFixture<KnjigaDodajComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ KnjigaDodajComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(KnjigaDodajComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
