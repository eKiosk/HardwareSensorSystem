import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { MatListModule, MatSidenavModule, MatToolbarModule } from '@angular/material';
import { RouterModule } from '@angular/router';

import { LayoutComponent } from './layout.component';

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    MatListModule,
    MatSidenavModule,
    MatToolbarModule
  ],
  declarations: [LayoutComponent]
})
export class LayoutModule { }
