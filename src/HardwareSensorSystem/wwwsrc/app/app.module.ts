import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { JwtModule } from '@auth0/angular-jwt';

import { AppComponent } from './app.component';
import { APP_ROUTES } from './app.routing';
import { ChartModule } from './chart/chart.module';
import { LayoutModule } from './layout/layout.module';
import { LoginModule } from './login/login.module';
import { LogoutModule } from './logout/logout.module';
import { AuthenticationService, SecurityModule } from './security';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    RouterModule.forRoot(APP_ROUTES),
    JwtModule.forRoot({
      config: {
        tokenGetter: () => sessionStorage.getItem(AuthenticationService.accessTokenName)
      }
    }),
    SecurityModule,
    LoginModule,
    LogoutModule,
    LayoutModule,
    ChartModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
