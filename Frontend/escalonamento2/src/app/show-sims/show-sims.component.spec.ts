import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShowSimsComponent } from './show-sims.component';

describe('ShowSimsComponent', () => {
  let component: ShowSimsComponent;
  let fixture: ComponentFixture<ShowSimsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShowSimsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShowSimsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
