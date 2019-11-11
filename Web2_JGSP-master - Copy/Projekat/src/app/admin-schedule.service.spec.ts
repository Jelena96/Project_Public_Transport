import { TestBed } from '@angular/core/testing';

import { AdminScheduleService } from './admin-schedule.service';

describe('AdminScheduleService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: AdminScheduleService = TestBed.get(AdminScheduleService);
    expect(service).toBeTruthy();
  });
});
