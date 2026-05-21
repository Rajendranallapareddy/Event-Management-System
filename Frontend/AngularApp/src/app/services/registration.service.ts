// src/app/services/registration.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Registration } from '../models/registration.model';
import { PagedResult } from '../models/paged-result.model';

@Injectable({ providedIn: 'root' })
export class RegistrationService {
  private baseUrl = `${environment.apiUrl}/registrations`;

  constructor(private http: HttpClient) {}

  registerForEvent(eventId: number): Observable<any> {
    return this.http.post(`${this.baseUrl}/register/${eventId}`, {});
  }

  getMyEvents(pageNumber: number, pageSize: number): Observable<PagedResult<Registration>> {
    let params = new HttpParams().set('pageNumber', pageNumber).set('pageSize', pageSize);
    return this.http.get<PagedResult<Registration>>(`${this.baseUrl}/my-events`, { params });
  }

  cancelRegistration(registrationId: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/cancel/${registrationId}`);
  }

  markAttendance(registrationId: number): Observable<any> {
    return this.http.patch(`${this.baseUrl}/attendance/${registrationId}`, {});
  }
  getAllRegistrations(page: number, size: number): Observable<PagedResult<Registration>> {
  let params = new HttpParams().set('pageNumber', page).set('pageSize', size);
  return this.http.get<PagedResult<Registration>>(`${this.baseUrl}/admin/all`, { params });
}
}