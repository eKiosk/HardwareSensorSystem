import { Routes } from '@angular/router';

import { CHART_ROUTES } from './chart/chart.routing';
import { LayoutComponent } from './layout/layout.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { ProtectedGuard } from './security';

export const APP_ROUTES: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'logout', component: LogoutComponent },
  {
    path: '', component: LayoutComponent, canActivate: [ProtectedGuard], children: [
      { path: 'charts', children: CHART_ROUTES },
      { path: 'devices', loadChildren: './device/device.module#DeviceModule' },
      { path: 'roles', loadChildren: './user/user.module#UserModule' }
    ]
  }
];
