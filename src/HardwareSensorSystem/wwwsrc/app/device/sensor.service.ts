import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { Sensor } from './sensor';

@Injectable()
export class SensorService {

  constructor(private httpClient: HttpClient) { }

  getAllByDeviceId(deviceId: number): Observable<Sensor[]> {
    return this.httpClient.get<Sensor[]>(`/api/devices/${deviceId}/sensors`);
  }

  getById(sensorId: number): Observable<Sensor> {
    return this.httpClient.get<Sensor>(`/api/sensors/${sensorId}`);
  }

  create(sensor: Sensor): Observable<Sensor> {
    return this.httpClient.post<Sensor>('/api/sensors', sensor);
  }

  update(sensor: Sensor): Observable<Sensor> {
    return this.httpClient.put<Sensor>(`/api/sensors/${sensor.id}`, sensor);
  }

  delete(sensor: Sensor): Observable<void> {
    return this.httpClient.delete<void>(`/api/sensors/${sensor.id}`);
  }

}
