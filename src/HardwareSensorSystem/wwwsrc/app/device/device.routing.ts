import { Routes } from '@angular/router';

import { DeviceDetailComponent } from './device-detail/device-detail.component';
import { DeviceListComponent } from './device-list/device-list.component';

export const DEVICE_ROUTES: Routes = [
  { path: '', component: DeviceListComponent },
  { path: ':id', component: DeviceDetailComponent }
];
