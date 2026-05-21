import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SpeakerService } from '../../services/speaker.service';
import { Speaker } from '../../models/speaker.model';

@Component({
  selector: 'app-speaker',
  templateUrl: './speaker.component.html'
})
export class SpeakerComponent implements OnInit {
  speakers: Speaker[] = [];
  loading = false;
  showModal = false;
  speakerForm: FormGroup;

  constructor(private speakerService: SpeakerService, private fb: FormBuilder) {
    this.speakerForm = this.fb.group({
      name: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadSpeakers();
  }

  loadSpeakers(): void {
    this.loading = true;
    this.speakerService.getSpeakers(1, 100).subscribe({
      next: (res) => { this.speakers = res.items; this.loading = false; },
      error: () => { this.loading = false; }
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
      },
      error: () => alert('Failed to add speaker')
    });
  }

  deleteSpeaker(id: number): void {
    if (confirm('Delete this speaker?')) {
      this.speakerService.deleteSpeaker(id).subscribe(() => this.loadSpeakers());
    }
  }
}