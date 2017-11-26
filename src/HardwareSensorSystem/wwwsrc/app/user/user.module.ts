import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { RoleListComponent } from './role-list/role-list.component';
import { USER_ROUTING } from './user.routing';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(USER_ROUTING)
  ],
  declarations: [RoleListComponent]
})
export class UserModule { }
