import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlorTicketsComponent } from './controlor-tickets.component';

describe('ControlorTicketsComponent', () => {
  let component: ControlorTicketsComponent;
  let fixture: ComponentFixture<ControlorTicketsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlorTicketsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlorTicketsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
