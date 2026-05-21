import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { Session } from '../models/session.model';
import { PagedResult } from '../models/paged-result.model';

@Injectable({ providedIn: 'root' })
export class SessionService {
  private baseUrl = `${environment.apiUrl}/sessions`;

  constructor(private http: HttpClient) {}

  getSessionsByEvent(eventId: number, pageNumber: number, pageSize: number): Observable<PagedResult<Session>> {
    const url = `${this.baseUrl}/event/${eventId}`;
    let params = new HttpParams().set('pageNumber', pageNumber).set('pageSize', pageSize);
    return this.http.get<PagedResult<Session>>(url, { params })
      .pipe(catchError(this.handleError));
  }

  getAllSessions(pageNumber: number, pageSize: number): Observable<PagedResult<Session>> {
    let params = new HttpParams().set('pageNumber', pageNumber).set('pageSize', pageSize);
    return this.http.get<PagedResult<Session>>(this.baseUrl, { params })
      .pipe(catchError(this.handleError));
  }

  createSession(session: any): Observable<Session> {
    return this.http.post<Session>(this.baseUrl, session);
  }

  assignSpeaker(sessionId: number, speakerId: number): Observable<any> {
    return this.http.patch(`${this.baseUrl}/${sessionId}/assign-speaker/${speakerId}`, {});
  }

  deleteSession(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

  private handleError(error: HttpErrorResponse) {
    console.error('HTTP Error Details:', {
      status: error.status,
      message: error.message,
      error: error.error
    });
    let errorMessage = 'Failed to load sessions';
    if (error.status === 0) {
      errorMessage = 'Backend server is not running. Please start the API.';
    } else if (error.status === 401) {
      errorMessage = 'Your session has expired. Please log in again.';
    } else if (error.status === 404) {
      errorMessage = 'No sessions found for this event. Please create sessions.';
    } else if (error.error?.message) {
      errorMessage = error.error.message;
    }
    return throwError(() => new Error(errorMessage));
  }
}