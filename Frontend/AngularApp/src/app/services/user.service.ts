import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { User } from '../models/user.model';
import { PagedResult } from '../models/paged-result.model';

@Injectable({ providedIn: 'root' })
export class UserService {
  private baseUrl = `${environment.apiUrl}/users`;

  constructor(private http: HttpClient) {}

  getAllUsers(page: number, size: number): Observable<PagedResult<User>> {
    let params = new HttpParams().set('pageNumber', page).set('pageSize', size);
    return this.http.get<PagedResult<User>>(this.baseUrl, { params });
  }
}