import { Routes } from '@angular/router';

import { DeviceDetailComponent } from './device-detail/device-detail.component';
import { DeviceListComponent } from './device-list/device-list.component';
import { SensorDetailComponent } from './sensor-detail/sensor-detail.component';
import { SensorListComponent } from './sensor-list/sensor-list.component';

export const DEVICE_ROUTES: Routes = [
  { path: '', component: DeviceListComponent },
  { path: ':id', component: DeviceDetailComponent },
  { path: ':id/sensors', component: SensorListComponent },
  { path: ':id/sensors/:sensorId', component: SensorDetailComponent }
];
