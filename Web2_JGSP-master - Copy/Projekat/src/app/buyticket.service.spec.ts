import { TestBed } from '@angular/core/testing';

import { BuyticketService } from './buyticket.service';

describe('BuyticketService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: BuyticketService = TestBed.get(BuyticketService);
    expect(service).toBeTruthy();
  });
});
