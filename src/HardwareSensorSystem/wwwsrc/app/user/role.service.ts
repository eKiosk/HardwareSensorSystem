import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { Role } from './role';

@Injectable()
export class RoleService {

  constructor(private httpClient: HttpClient) { }

  getAll(): Observable<Role[]> {
    return this.httpClient.get<Role[]>('/api/roles');
  }

  getById(roleId: number): Observable<Role> {
    return this.httpClient.get<Role>(`/api/roles/${roleId}`);
  }

  create(role: Role): Observable<Role> {
    return this.httpClient.post<Role>('/api/roles', role);
  }

  update(role: Role): Observable<Role> {
    return this.httpClient.put<Role>(`/api/roles/${role.id}`, role);
  }

}
