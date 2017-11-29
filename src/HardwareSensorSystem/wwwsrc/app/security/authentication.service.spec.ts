import { HttpClientTestingModule } from '@angular/common/http/testing';
import { inject, TestBed } from '@angular/core/testing';

import { AuthenticationService } from './authentication.service';
import { JwtHelperService, JWT_OPTIONS } from '@auth0/angular-jwt';

describe('AuthenticationService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        HttpClientTestingModule
      ],
      providers: [
        JwtHelperService,
        {
          provide: JWT_OPTIONS,
          useValue: {}
        },
        AuthenticationService
      ]
    });
  });

  it('should be created', inject([AuthenticationService], (service: AuthenticationService) => {
    expect(service).toBeTruthy();
  }));
});
