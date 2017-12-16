import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';

import { User } from './user';

@Injectable()
export class UserService {

  constructor(private httpClient: HttpClient) { }

  getAllByRoleId(roleId: number): Observable<User[]> {
    return this.httpClient.get<User[]>(`/api/roles/${roleId}/users`);
  }

  getById(userId: number): Observable<User> {
    return this.httpClient.get<User>(`/api/users/${userId}`);
  }

  create(user: User): Observable<User> {
    return this.httpClient.post<User>('/api/users', user);
  }

  update(user: User): Observable<User> {
    return this.httpClient.put<User>(`/api/users/${user.id}`, user);
  }

  delete(user: User): Observable<void> {
    return this.httpClient.delete<void>(`/api/users/${user.id}`);
  }

}
