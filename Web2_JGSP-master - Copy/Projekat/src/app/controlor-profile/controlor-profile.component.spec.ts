import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ControlorProfileComponent } from './controlor-profile.component';

describe('ControlorProfileComponent', () => {
  let component: ControlorProfileComponent;
  let fixture: ComponentFixture<ControlorProfileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ControlorProfileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ControlorProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
