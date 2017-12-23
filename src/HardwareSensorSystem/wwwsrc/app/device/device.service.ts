import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { Device } from './device';

@Injectable()
export class DeviceService {

  constructor(private httpClient: HttpClient) { }

  getAll(): Observable<Device[]> {
    return this.httpClient.get<Device[]>('/api/devices');
  }

  getById(deviceId: number): Observable<Device> {
    return this.httpClient.get<Device>(`/api/devices/${deviceId}`);
  }

  create(device: Device): Observable<Device> {
    return this.httpClient.post<Device>('/api/devices', device);
  }

  update(device: Device): Observable<Device> {
    return this.httpClient.put<Device>(`/api/devices/${device.id}`, device);
  }

  delete(device: Device): Observable<void> {
    return this.httpClient.delete<void>(`/api/devices/${device.id}`);
  }

}
