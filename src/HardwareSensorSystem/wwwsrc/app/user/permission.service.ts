import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { Permission } from './permission';

@Injectable()
export class PermissionService {

  constructor(private httpClient: HttpClient) { }

  getAll(): Observable<Permission[]> {
    return this.httpClient.get<Permission[]>('/api/permissions');
  }

  getIdsByRole(roleId: number): Observable<number[]> {
    return this.httpClient.get<number[]>(`/api/roles/${roleId}/permissions`);
  }

  addToRole(roleId: number, permissionId: number): Observable<void> {
    return this.httpClient.post<void>(`/api/roles/${roleId}/permissions/${permissionId}`, null);
  }

  removeFromRole(roleId: number, permissionId: number): Observable<void> {
    return this.httpClient.delete<void>(`/api/roles/${roleId}/permissions/${permissionId}`);
  }

}
