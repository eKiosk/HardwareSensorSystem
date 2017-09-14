import { Routes, RouterModule } from '@angular/router';

import { RoleListComponent } from './role-list/role-list.component';

const routes: Routes = [
  { path: '', component: RoleListComponent }
];

export const UserRouting = RouterModule.forChild(routes);
