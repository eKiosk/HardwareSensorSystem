import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs/Observable';
import { fromPromise } from 'rxjs/observable/fromPromise';
import { _throw } from 'rxjs/observable/throw';
import { catchError, map } from 'rxjs/operators';
import { noop } from 'rxjs/util/noop';

@Injectable()
export class AuthenticationService {

  constructor(
    private oauthService: OAuthService
  ) {
    oauthService.tokenEndpoint = '/connect/token';
    oauthService.clientId = 'hardwaresensorsystem';
    oauthService.scope = 'offline_access';
    oauthService.setStorage(sessionStorage);

    if (oauthService.getRefreshToken()) {
      oauthService.refreshToken();
    }

    oauthService.events.filter(e => e.type === 'token_expires').subscribe(e => {
      oauthService.refreshToken();
    });
  }

  login(userName: string, password: string): Observable<void> {
    return fromPromise(this.oauthService.fetchTokenUsingPasswordFlow(userName, password)).pipe(
      map(noop),
      catchError(error => {
        return _throw('Service steht im Moment leider nicht zur Verfügung');
      })
    );
  }

  logout() {
    this.oauthService.logOut();
  }

  isAuthenticated(): boolean {
    return this.oauthService.hasValidAccessToken();
  }

}
