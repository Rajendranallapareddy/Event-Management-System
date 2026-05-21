// src/app/services/speaker.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { Speaker } from '../models/speaker.model';
import { PagedResult } from '../models/paged-result.model';

@Injectable({ providedIn: 'root' })
export class SpeakerService {
  private baseUrl = `${environment.apiUrl}/speakers`;

  constructor(private http: HttpClient) {}

  getSpeakers(page: number, size: number, search?: string): Observable<PagedResult<Speaker>> {
    let params = new HttpParams().set('pageNumber', page).set('pageSize', size);
    if (search) params = params.set('searchTerm', search);
    return this.http.get<PagedResult<Speaker>>(this.baseUrl, { params });
  }

  createSpeaker(speaker: any): Observable<Speaker> {
    return this.http.post<Speaker>(this.baseUrl, speaker);
  }

  deleteSpeaker(id: number): Observable<any> {
    console.log(`Deleting speaker with ID: ${id}, URL: ${this.baseUrl}/${id}`);
    return this.http.delete(`${this.baseUrl}/${id}`).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: HttpErrorResponse) {
    console.error('Delete error details:', error);
    let errorMessage = 'Unknown error';
    if (error.error instanceof ErrorEvent) {
      // client-side error
      errorMessage = `Client error: ${error.error.message}`;
    } else {
      // server-side error
      errorMessage = `Server returned code ${error.status}: ${error.message}`;
      if (error.status === 401) errorMessage = 'Unauthorized. Please login again.';
      else if (error.status === 404) errorMessage = 'Speaker not found.';
      else if (error.status === 500) errorMessage = 'Internal server error. Check backend logs.';
    }
    return throwError(() => new Error(errorMessage));
  }
}