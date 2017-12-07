import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import {
  MatButtonModule,
  MatFormFieldModule,
  MatIconModule,
  MatIconRegistry,
  MatInputModule,
  MatPaginatorModule,
  MatSortModule,
  MatTableModule
} from '@angular/material';
import { RouterModule } from '@angular/router';

import { RoleDetailComponent } from './role-detail/role-detail.component';
import { RoleListComponent } from './role-list/role-list.component';
import { RoleService } from './role.service';
import { USER_ROUTING } from './user.routing';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(USER_ROUTING),
    ReactiveFormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatPaginatorModule,
    MatSortModule,
    MatTableModule
  ],
  declarations: [
    RoleListComponent,
    RoleDetailComponent
  ],
  providers: [
    RoleService
  ]
})
export class UserModule {
  constructor(iconRegistry: MatIconRegistry) {
    iconRegistry.registerFontClassAlias('fontawesome', 'fa');
  }
}
