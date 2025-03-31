import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BorrowerscreenComponent } from './borrowerscreen.component';

describe('BorrowerscreenComponent', () => {
  let component: BorrowerscreenComponent;
  let fixture: ComponentFixture<BorrowerscreenComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [BorrowerscreenComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(BorrowerscreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
