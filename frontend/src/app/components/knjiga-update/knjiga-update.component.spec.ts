import { ComponentFixture, TestBed } from '@angular/core/testing';

import { KnjigaUpdateComponent } from './knjiga-update.component';

describe('KnjigaUpdateComponent', () => {
  let component: KnjigaUpdateComponent;
  let fixture: ComponentFixture<KnjigaUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ KnjigaUpdateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(KnjigaUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
