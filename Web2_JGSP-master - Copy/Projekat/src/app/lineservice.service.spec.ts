import { TestBed } from '@angular/core/testing';

import { LineserviceService } from './lineservice.service';

describe('LineserviceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LineserviceService = TestBed.get(LineserviceService);
    expect(service).toBeTruthy();
  });
});
