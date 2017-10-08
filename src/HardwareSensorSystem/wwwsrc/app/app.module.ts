import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LayoutComponent } from './layout/layout.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { UiComponentsModule } from './ui-components/ui-components.module';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    LogoutComponent,
    LayoutComponent
  ],
  imports: [
    BrowserModule,
    ReactiveFormsModule,
    AppRoutingModule,
    UiComponentsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
