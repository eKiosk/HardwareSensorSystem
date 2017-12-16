import { Routes } from '@angular/router';

import { PermissionListComponent } from './permission-list/permission-list.component';
import { RoleDetailComponent } from './role-detail/role-detail.component';
import { RoleListComponent } from './role-list/role-list.component';
import { UserDetailComponent } from './user-detail/user-detail.component';
import { UserListComponent } from './user-list/user-list.component';

export const USER_ROUTING: Routes = [
  { path: '', component: RoleListComponent },
  { path: ':id', component: RoleDetailComponent },
  { path: ':id/permissions', component: PermissionListComponent },
  { path: ':id/users', component: UserListComponent },
  { path: ':id/users/:userId', component: UserDetailComponent }
];
