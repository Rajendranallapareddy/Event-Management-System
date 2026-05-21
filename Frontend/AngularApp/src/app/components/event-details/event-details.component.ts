import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { EventService } from '../../services/event.service';
import { SessionService } from '../../services/session.service';
import { RegistrationService } from '../../services/registration.service';
import { AuthService } from '../../services/auth.service';
import { Event } from '../../models/event.model';
import { Session } from '../../models/session.model';

@Component({
  selector: 'app-event-details',
  templateUrl: './event-details.component.html',
  styleUrls: ['./event-details.component.css']
})
export class EventDetailsComponent implements OnInit {
  event: Event | null = null;
  sessions: Session[] = [];
  loading = true;
  error = '';
  registered = false;

  constructor(
    private route: ActivatedRoute,
    private eventService: EventService,
    private sessionService: SessionService,
    private registrationService: RegistrationService,
    public authService: AuthService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if (id) {
      this.loadEvent(id);
      this.loadSessions(id);
      this.checkRegistration(id);
    }
  }

  loadEvent(id: number): void {
    this.eventService.getEventById(id).subscribe({
      next: (data) => { this.event = data; this.loading = false; },
      error: (err) => { this.error = 'Failed to load event details'; this.loading = false; }
    });
  }

  loadSessions(eventId: number): void {
    this.sessionService.getSessionsByEvent(eventId, 1, 100).subscribe({
      next: (res) => { this.sessions = res.items; }
    });
  }

  checkRegistration(eventId: number): void {
    if (!this.authService.isAuthenticated()) return;
    this.registrationService.getMyEvents(1, 100).subscribe({
      next: (res) => {
        this.registered = res.items.some(r => r.eventId === eventId);
      }
    });
  }

  register(): void {
    if (!this.event) return;
    this.registrationService.registerForEvent(this.event.eventId).subscribe({
      next: () => {
        this.registered = true;
        alert('Successfully registered for the event!');
      },
      error: (err) => { alert(err.error?.message || 'Registration failed'); }
    });
  }
}