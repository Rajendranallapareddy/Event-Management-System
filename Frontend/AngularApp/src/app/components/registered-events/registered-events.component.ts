import { Component, OnInit } from '@angular/core';
import { RegistrationService } from '../../services/registration.service';
import { SessionService } from '../../services/session.service';
import { Registration } from '../../models/registration.model';
import { Session } from '../../models/session.model';

@Component({
  selector: 'app-registered-events',
  templateUrl: './registered-events.component.html'
})
export class RegisteredEventsComponent implements OnInit {
  registrations: Registration[] = [];
  loading = true;
  showSessionsModal = false;
  selectedEventId = 0;
  selectedEventTitle = '';
  sessions: Session[] = [];
  sessionsLoading = false;
  sessionsError = '';

  constructor(
    private registrationService: RegistrationService,
    private sessionService: SessionService
  ) {}

  ngOnInit(): void {
    this.loadRegistrations();
  }

  loadRegistrations(): void {
    this.loading = true;
    this.registrationService.getMyEvents(1, 100).subscribe({
      next: (res) => {
        this.registrations = res.items;
        this.loading = false;
        console.log('Registered events:', this.registrations);
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
        alert('Failed to load registered events');
      }
    });
  }

  cancelRegistration(regId: number): void {
    if (confirm('Cancel registration?')) {
      this.registrationService.cancelRegistration(regId).subscribe({
        next: () => this.loadRegistrations(),
        error: () => alert('Cancellation failed')
      });
    }
  }

  viewSessions(eventId: number, eventTitle: string): void {
    this.selectedEventId = eventId;
    this.selectedEventTitle = eventTitle;
    this.sessionsLoading = true;
    this.sessionsError = '';
    this.sessions = [];

    console.log(`Fetching sessions for event ID: ${eventId}`);

    this.sessionService.getSessionsByEvent(eventId, 1, 100).subscribe({
      next: (res) => {
        console.log('Sessions API response:', res);
        this.sessions = res.items;
        this.sessionsLoading = false;
        this.showSessionsModal = true;
      },
      error: (err) => {
        console.error('Session load error:', err);
        this.sessionsLoading = false;
        this.sessionsError = err.message;
        this.showSessionsModal = true;
      }
    });
  }

  closeModal(): void {
    this.showSessionsModal = false;
    this.sessions = [];
    this.sessionsError = '';
  }
}