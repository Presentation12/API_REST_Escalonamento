import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewSimTemposComponent } from './new-sim-tempos.component';

describe('NewSimTemposComponent', () => {
  let component: NewSimTemposComponent;
  let fixture: ComponentFixture<NewSimTemposComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NewSimTemposComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(NewSimTemposComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
