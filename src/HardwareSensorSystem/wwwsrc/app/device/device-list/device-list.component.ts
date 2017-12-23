import {
  AfterViewInit,
  Component,
  OnInit,
  ViewChild
} from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';

import { Device } from '../device';
import { DeviceService } from '../device.service';

@Component({
  selector: 'app-device-list',
  templateUrl: './device-list.component.html',
  styles: [`
  .bar {
    padding-bottom: 1em;
  }`]
})
export class DeviceListComponent implements OnInit, AfterViewInit {
  displayedColumns = ['name', 'actions'];
  dataSource: MatTableDataSource<Device>;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  constructor(
    private deviceService: DeviceService
  ) {
    this.dataSource = new MatTableDataSource<Device>([]);
  }

  ngOnInit() {
    this.deviceService.getAll().subscribe(roles => {
      this.dataSource.data = roles;
    });
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  deleteDevice(device: Device) {
    if (confirm(`Delete device '${device.name}'?`)) {
      this.deviceService.delete(device).subscribe(() => {
        this.dataSource.data = this.dataSource.data.filter(dataRole => dataRole.id !== device.id);
      });
    }
  }

}
