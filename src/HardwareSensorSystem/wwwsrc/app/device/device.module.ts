import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DeviceRouting } from "./device.routing";
import { DeviceListComponent } from './device-list/device-list.component';

@NgModule({
  imports: [
    CommonModule,
    DeviceRouting
  ],
  declarations: [DeviceListComponent]
})
export class DeviceModule { }
