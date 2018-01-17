import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { ReactiveFormsModule } from '@angular/forms';
import {
  MatButtonModule,
  MatFormFieldModule,
  MatIconModule,
  MatIconRegistry,
  MatInputModule,
  MatPaginatorModule,
  MatSlideToggleModule,
  MatSortModule,
  MatTableModule
} from '@angular/material';
import { RouterModule } from '@angular/router';

import { DeviceDetailComponent } from './device-detail/device-detail.component';
import { DeviceListComponent } from './device-list/device-list.component';
import { DEVICE_ROUTES } from './device.routing';
import { DeviceService } from './device.service';
import { SensorDetailComponent } from './sensor-detail/sensor-detail.component';
import { SensorListComponent } from './sensor-list/sensor-list.component';
import { SensorService } from './sensor.service';

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(DEVICE_ROUTES),
    ReactiveFormsModule,
    FlexLayoutModule,
    MatButtonModule,
    MatFormFieldModule,
    MatIconModule,
    MatInputModule,
    MatPaginatorModule,
    MatSlideToggleModule,
    MatSortModule,
    MatTableModule
  ],
  declarations: [
    DeviceListComponent,
    DeviceDetailComponent,
    SensorListComponent,
    SensorDetailComponent
  ],
  providers: [
    DeviceService,
    SensorService
  ]
})
export class DeviceModule {
  constructor(iconRegistry: MatIconRegistry) {
    iconRegistry.registerFontClassAlias('fontawesome', 'fa');
  }
}
