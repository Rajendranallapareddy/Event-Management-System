import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SessionService } from '../../services/session.service';
import { EventService } from '../../services/event.service';
import { SpeakerService } from '../../services/speaker.service';
import { Session } from '../../models/session.model';
import { Event } from '../../models/event.model';
import { Speaker } from '../../models/speaker.model';

@Component({
  selector: 'app-session',
  templateUrl: './session.component.html'
})
export class SessionComponent implements OnInit {
  sessions: Session[] = [];
  events: Event[] = [];
  speakers: Speaker[] = [];
  loading = false;
  showCreateModal = false;
  showAssignModal = false;
  selectedSessionId = 0;
  sessionForm: FormGroup;
  assignForm: FormGroup;

  constructor(
    private sessionService: SessionService,
    private eventService: EventService,
    private speakerService: SpeakerService,
    private fb: FormBuilder
  ) {
    this.sessionForm = this.fb.group({
      eventId: [0, Validators.required],
      title: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', Validators.maxLength(1000)],
      speakerId: [null],
      sessionStart: ['', Validators.required],
      sessionEnd: ['', Validators.required]
    });
    this.assignForm = this.fb.group({
      speakerId: [0, Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadSessions();
    this.loadEvents();
    this.loadSpeakers();
  }

  loadSessions(): void {
    this.loading = true;
    this.sessionService.getSessionsByEvent(0, 1, 100).subscribe({
      next: (res) => { this.sessions = res.items; this.loading = false; },
      error: () => this.loading = false
    });
  }

  loadEvents(): void {
    this.eventService.getEvents(1, 100).subscribe(res => this.events = res.items);
  }

  loadSpeakers(): void {
    this.speakerService.getSpeakers(1, 100).subscribe(res => this.speakers = res.items);
  }

  openCreateModal(): void {
    this.sessionForm.reset({ eventId: 0, title: '', description: '', speakerId: null, sessionStart: '', sessionEnd: '' });
    this.showCreateModal = true;
  }

  createSession(): void {
    if (this.sessionForm.invalid) return;
    this.sessionService.createSession(this.sessionForm.value).subscribe({
      next: () => { this.showCreateModal = false; this.loadSessions(); },
      error: (err) => alert(err.error?.message || 'Failed to create session')
    });
  }

  openAssignModal(sessionId: number): void {
    this.selectedSessionId = sessionId;
    this.assignForm.reset({ speakerId: 0 });
    this.showAssignModal = true;
  }

  assignSpeaker(): void {
    if (this.assignForm.invalid) return;
    this.sessionService.assignSpeaker(this.selectedSessionId, this.assignForm.value.speakerId).subscribe({
      next: () => { this.showAssignModal = false; this.loadSessions(); },
      error: () => alert('Assignment failed')
    });
  }

  deleteSession(id: number): void {
    if (confirm('Delete this session?')) {
      this.sessionService.deleteSession(id).subscribe(() => this.loadSessions());
    }
  }
}