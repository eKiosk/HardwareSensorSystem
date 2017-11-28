import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { AuthenticationService } from './authentication.service';
import { ProtectedGuard } from './protected.guard';

@NgModule({
  imports: [
    CommonModule
  ],
  providers: [
    AuthenticationService,
    ProtectedGuard
  ]
})
export class SecurityModule { }
