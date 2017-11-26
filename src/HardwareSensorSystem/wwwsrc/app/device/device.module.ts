import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { DeviceListComponent } from './device-list/device-list.component';
import { DEVICE_ROUTES } from './device.routing';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(DEVICE_ROUTES)
  ],
  declarations: [DeviceListComponent]
})
export class DeviceModule { }
