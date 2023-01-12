import { TestBed } from '@angular/core/testing';

import { CatalogReadService } from './catalog-read.service';

describe('CatalogReadService', () => {
  let service: CatalogReadService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CatalogReadService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
