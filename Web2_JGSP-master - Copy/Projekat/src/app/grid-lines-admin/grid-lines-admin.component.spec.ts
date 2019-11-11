import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GridLinesAdminComponent } from './grid-lines-admin.component';

describe('GridLinesAdminComponent', () => {
  let component: GridLinesAdminComponent;
  let fixture: ComponentFixture<GridLinesAdminComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GridLinesAdminComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GridLinesAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
