import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChartRouting } from "./chart.routing";
import { ChartListComponent } from './chart-list/chart-list.component';

@NgModule({
  imports: [
    CommonModule,
    ChartRouting
  ],
  declarations: [ChartListComponent]
})
export class ChartModule { }
