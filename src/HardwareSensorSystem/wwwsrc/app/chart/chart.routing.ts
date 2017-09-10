import { Routes, RouterModule } from '@angular/router';

import { ChartListComponent } from "./chart-list/chart-list.component";

const routes: Routes = [
  { path: '', component: ChartListComponent }
]

export const ChartRouting = RouterModule.forChild(routes);
