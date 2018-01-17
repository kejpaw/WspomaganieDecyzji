import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AllMarkersComponent } from './all-markers.component';

describe('AllMarkersComponent', () => {
  let component: AllMarkersComponent;
  let fixture: ComponentFixture<AllMarkersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AllMarkersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AllMarkersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
