import { AfterViewInit, Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, MatSort, MatTableDataSource } from '@angular/material';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs/Subscription';

import { Sensor } from '../sensor';
import { SensorService } from '../sensor.service';

@Component({
  selector: 'app-sensor-list',
  templateUrl: './sensor-list.component.html',
  styles: [`
  .bar {
    padding-bottom: 1em;
  }`]
})
export class SensorListComponent implements OnInit, AfterViewInit {
  displayedColumns = ['name', 'actions'];
  dataSource: MatTableDataSource<Sensor>;
  deviceId: number;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  private subscription: Subscription;

  constructor(
    private sensorService: SensorService,
    private route: ActivatedRoute
  ) {
    this.dataSource = new MatTableDataSource<Sensor>([]);
  }

  ngOnInit() {
    this.subscription = this.route.params.subscribe(params => {
      this.deviceId = params.id;
      this.sensorService.getAllByDeviceId(this.deviceId).subscribe(sensors => {
        this.dataSource.data = sensors;
      });
    });
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  deleteSensor(sensor: Sensor) {
    if (confirm(`Delete sensor '${sensor.name}'?`)) {
      this.sensorService.delete(sensor).subscribe(() => {
        this.dataSource.data = this.dataSource.data.filter(dataSensor => dataSensor.id !== sensor.id);
      });
    }
  }

}
