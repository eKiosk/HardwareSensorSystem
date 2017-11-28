import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { AuthenticationService } from './authentication.service';

@NgModule({
  imports: [
    CommonModule
  ],
  providers: [
    AuthenticationService
  ]
})
export class SecurityModule { }
