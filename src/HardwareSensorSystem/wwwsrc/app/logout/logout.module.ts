import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { LogoutComponent } from './logout.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule
  ],
  declarations: [LogoutComponent]
})
export class LogoutModule { }
