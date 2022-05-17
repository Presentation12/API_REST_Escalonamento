import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SimgeneratorComponent } from './simgenerator.component';

describe('SimgeneratorComponent', () => {
  let component: SimgeneratorComponent;
  let fixture: ComponentFixture<SimgeneratorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SimgeneratorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(SimgeneratorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
