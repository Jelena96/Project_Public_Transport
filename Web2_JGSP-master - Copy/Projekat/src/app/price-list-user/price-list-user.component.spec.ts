import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PriceListUserComponent } from './price-list-user.component';

describe('PriceListUserComponent', () => {
  let component: PriceListUserComponent;
  let fixture: ComponentFixture<PriceListUserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PriceListUserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PriceListUserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
