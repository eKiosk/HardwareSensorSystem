import 'rxjs/add/operator/catch';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { interval } from 'rxjs/observable/interval';
import { map } from 'rxjs/operators';
import { Subscription } from 'rxjs/Subscription';

import { Token } from './token';

@Injectable()
export class AuthenticationService {
  private static readonly accessTokenName = 'accessToken';
  private static readonly refreshTokenName = 'refreshToken';

  private headers: HttpHeaders;
  private refreshSubscription: Subscription;

  constructor(private httpClient: HttpClient) {
    this.headers = new HttpHeaders({ 'Content-Type': 'application/x-www-form-urlencoded' });

    if (sessionStorage.getItem(AuthenticationService.refreshTokenName)) {
      this.refreshToken();
    }
  }

  login(username: string, password: string): Observable<void> {
    const body = new HttpParams();
    body.set('grant_type', 'password');
    body.set('client_id', 'hardwaresensorsystem');
    body.set('scope', 'offline_access');
    body.set('username', username);
    body.set('password', password);

    return this.httpClient.post<Token>('/connect/token', body, { headers: this.headers }).pipe(
      map(token => {
        this.setToken(token);
        this.refreshSubscription = interval((token.expiresIn - 120) * 1000).subscribe(() => {
          this.refreshToken();
        });
        return;
      })
    ).catch(error => {
      return Observable.throw('Service steht im Moment leider nicht zur Verfügung');
    });
  }

  logout() {
    if (!this.refreshSubscription) {
      this.refreshSubscription.unsubscribe();
    }

    sessionStorage.removeItem(AuthenticationService.refreshTokenName);
    sessionStorage.removeItem(AuthenticationService.accessTokenName);
  }

  private refreshToken() {
    const body = new HttpParams();
    body.set('grant_type', 'refresh_token');
    body.set('client_id', 'hardwaresensorsystem');
    body.set('scope', 'offline_access');
    body.set('refresh_token', sessionStorage.getItem(AuthenticationService.refreshTokenName));

    this.httpClient.post<Token>('/connect/token', body, { headers: this.headers }).subscribe(token => {
      this.setToken(token);
      if (this.refreshSubscription) {
        this.refreshSubscription = interval((token.expiresIn - 120) * 1000).subscribe(() => {
          this.refreshToken();
        });
      }
    });
  }

  private setToken(token: Token) {
    sessionStorage.setItem(AuthenticationService.accessTokenName, token.accessToken);
    sessionStorage.setItem(AuthenticationService.refreshTokenName, token.refreshToken);
  }

}
