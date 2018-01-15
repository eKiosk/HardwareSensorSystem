import { HttpClientTestingModule } from '@angular/common/http/testing';
import { inject, TestBed } from '@angular/core/testing';

import { DeviceService } from './device.service';

describe('DeviceService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      providers: [DeviceService]
    });
  });

  it('should be created', inject([DeviceService], (service: DeviceService) => {
    expect(service).toBeTruthy();
  }));
});
