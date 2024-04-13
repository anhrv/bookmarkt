import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RecenzijeComponent } from './recenzije.component';

describe('RecenzijeComponent', () => {
  let component: RecenzijeComponent;
  let fixture: ComponentFixture<RecenzijeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RecenzijeComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RecenzijeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
