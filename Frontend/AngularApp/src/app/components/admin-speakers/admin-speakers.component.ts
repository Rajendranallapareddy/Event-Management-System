// src/app/components/admin-speakers/admin-speakers.component.ts
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SpeakerService } from '../../services/speaker.service';
import { Speaker } from '../../models/speaker.model';

@Component({
  selector: 'app-admin-speakers',
  templateUrl: './admin-speakers.component.html'
})
export class AdminSpeakersComponent implements OnInit {
  speakers: Speaker[] = [];
  loading = false;
  showModal = false;
  speakerForm: FormGroup;

  constructor(private speakerService: SpeakerService, private fb: FormBuilder) {
    this.speakerForm = this.fb.group({
      name: ['', Validators.required],
      bio: ['', Validators.maxLength(500)],
      company: ['', Validators.maxLength(100)],
      designation: ['', Validators.maxLength(100)]
    });
  }

  ngOnInit(): void {
    this.loadSpeakers();
  }

  loadSpeakers(): void {
    this.loading = true;
    this.speakerService.getSpeakers(1, 100).subscribe({
      next: (res) => { this.speakers = res.items; this.loading = false; },
      error: (err) => {
        console.error('Load speakers error', err);
        alert('Failed to load speakers. Check network.');
        this.loading = false;
      }
    });
  }

  openModal(): void {
    this.speakerForm.reset();
    this.showModal = true;
  }

  addSpeaker(): void {
    if (this.speakerForm.invalid) return;
    this.speakerService.createSpeaker(this.speakerForm.value).subscribe({
      next: () => {
        this.showModal = false;
        this.loadSpeakers();
        alert('Speaker added successfully');
      },
      error: (err) => {
        console.error('Add speaker error', err);
        alert(err.message || 'Failed to add speaker');
      }
    });
  }

  deleteSpeaker(id: number): void {
    if (!confirm('Delete this speaker? This action cannot be undone.')) return;
    this.speakerService.deleteSpeaker(id).subscribe({
      next: () => {
        this.loadSpeakers();
        alert('Speaker deleted successfully');
      },
      error: (err) => {
        console.error('Delete error:', err);
        alert(`Delete failed: ${err.message}`);
      }
    });
  }
}