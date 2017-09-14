import { Routes, RouterModule } from '@angular/router';

import { DeviceListComponent } from './device-list/device-list.component';

const routes = [
  { path: '', component: DeviceListComponent }
];

export const DeviceRouting = RouterModule.forChild(routes);
