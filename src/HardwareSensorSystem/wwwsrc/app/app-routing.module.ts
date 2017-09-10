import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { LayoutComponent } from './layout/layout.component';
import { ChartListComponent } from './chart/chart-list/chart-list.component';
import { DeviceListComponent } from './device/device-list/device-list.component';

const routes: Routes = [
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'logout', component: LogoutComponent },
  {
    path: '', component: LayoutComponent, children: [
      { path: 'charts', loadChildren: './chart/chart.module#ChartModule' },
      { path: 'devices', loadChildren: './device/device.module#DeviceModule' },
      { path: 'roles', loadChildren: './user/user.module#UserModule' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
