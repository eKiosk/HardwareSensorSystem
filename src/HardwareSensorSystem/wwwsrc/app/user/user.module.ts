import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserRouting } from './user.routing';
import { RoleListComponent } from './role-list/role-list.component';

@NgModule({
  imports: [
    CommonModule,
    UserRouting
  ],
  declarations: [RoleListComponent]
})
export class UserModule { }
