import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SessionService } from '../../services/session.service';
import { EventService } from '../../services/event.service';
import { SpeakerService } from '../../services/speaker.service';
import { Session } from '../../models/session.model';
import { Event } from '../../models/event.model';
import { Speaker } from '../../models/speaker.model';

@Component({
  selector: 'app-admin-sessions',
  templateUrl: './admin-sessions.component.html'
})
export class AdminSessionsComponent implements OnInit {
  sessions: Session[] = [];
  events: Event[] = [];
  speakers: Speaker[] = [];
  loading = false;
  showCreateModal = false;
  showAssignModal = false;
  selectedSessionId = 0;
  sessionForm: FormGroup;
  assignForm: FormGroup;
  currentPage = 1;
  pageSize = 10;
  totalPages = 0;

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
    this.sessionService.getAllSessions(this.currentPage, this.pageSize).subscribe({
      next: (res: any) => {
        this.sessions = res.items;
        this.totalPages = res.totalPages;
        this.loading = false;
      },
      error: (err: any) => {
        console.error(err);
        this.loading = false;
      }
    });
  }

  loadEvents(): void {
    this.eventService.getEvents(1, 100).subscribe({
      next: (res: any) => this.events = res.items,
      error: (err: any) => console.error(err)
    });
  }

  loadSpeakers(): void {
    this.speakerService.getSpeakers(1, 100).subscribe({
      next: (res: any) => this.speakers = res.items,
      error: (err: any) => console.error(err)
    });
  }

  openCreateModal(): void {
    this.sessionForm.reset({ eventId: 0, title: '', description: '', speakerId: null, sessionStart: '', sessionEnd: '' });
    this.showCreateModal = true;
  }

  createSession(): void {
    if (this.sessionForm.invalid) {
      alert('Please fill all required fields');
      return;
    }
    const formValue = this.sessionForm.value;
    const startDate = new Date(formValue.sessionStart);
    const endDate = new Date(formValue.sessionEnd);
    if (endDate <= startDate) {
      alert('End time must be after start time');
      return;
    }
    const payload = {
      EventId: formValue.eventId,
      Title: formValue.title,
      Description: formValue.description,
      SpeakerId: formValue.speakerId === '' ? null : formValue.speakerId,
      SessionStart: startDate.toISOString(),
      SessionEnd: endDate.toISOString()
    };
    this.sessionService.createSession(payload).subscribe({
      next: () => {
        this.showCreateModal = false;
        this.loadSessions();
        alert('Session created successfully');
      },
      error: (err: any) => alert(err.error?.message || 'Failed to create session')
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
      next: () => {
        this.showAssignModal = false;
        this.loadSessions();
        alert('Speaker assigned successfully');
      },
      error: (err: any) => alert(err.error?.message || 'Assignment failed')
    });
  }

  deleteSession(id: number): void {
    if (confirm('Delete this session?')) {
      this.sessionService.deleteSession(id).subscribe({
        next: () => this.loadSessions(),
        error: (err: any) => alert('Delete failed')
      });
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadSessions();
    }
  }

  nextPage(): void {
    if (this.currentPage < this.totalPages) {
      this.currentPage++;
      this.loadSessions();
    }
  }
}