import { Routes } from '@angular/router';

import { RoleDetailComponent } from './role-detail/role-detail.component';
import { RoleListComponent } from './role-list/role-list.component';

export const USER_ROUTING: Routes = [
  { path: '', component: RoleListComponent },
  { path: ':id', component: RoleDetailComponent }
];
