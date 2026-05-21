// src/app/components/event-form/event-form.component.ts
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators, AbstractControl, ValidationErrors } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { EventService } from '../../services/event.service';

@Component({
  selector: 'app-event-form',
  templateUrl: './event-form.component.html'
})
export class EventFormComponent implements OnInit {
  eventForm: FormGroup;
  isEdit = false;
  eventId = 0;
  loading = false;
  error = '';

  // Custom validator: end date must be after start date
  static dateRangeValidator(group: AbstractControl): ValidationErrors | null {
    const start = group.get('startDate')?.value;
    const end = group.get('endDate')?.value;
    if (start && end && new Date(end) <= new Date(start)) {
      return { dateRange: true };
    }
    return null;
  }

  constructor(
    private fb: FormBuilder,
    private eventService: EventService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.eventForm = this.fb.group({
      title: ['', [Validators.required, Validators.maxLength(200)]],
      description: ['', [Validators.required, Validators.maxLength(1000)]],
      location: ['', [Validators.required, Validators.maxLength(200)]],
      startDate: ['', Validators.required],
      endDate: ['', Validators.required],
      capacity: [0, [Validators.required, Validators.min(1)]],
      imageUrl: ['']
    }, { validators: EventFormComponent.dateRangeValidator });
  }

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEdit = true;
      this.eventId = +id;
      this.loadEvent();
    }
  }

  loadEvent(): void {
    this.eventService.getEventById(this.eventId).subscribe({
      next: (event) => {
        this.eventForm.patchValue({
          title: event.title,
          description: event.description,
          location: event.location,
          startDate: this.formatDate(event.startDate),
          endDate: this.formatDate(event.endDate),
          capacity: event.capacity,
          imageUrl: event.imageUrl
        });
      },
      error: () => this.error = 'Failed to load event'
    });
  }

  formatDate(date: Date): string {
    const d = new Date(date);
    return d.toISOString().slice(0, 16);
  }

  onSubmit(): void {
    if (this.eventForm.invalid) {
      // Mark all fields as touched to display validation messages
      Object.keys(this.eventForm.controls).forEach(key => {
        this.eventForm.get(key)?.markAsTouched();
      });
      return;
    }
    this.loading = true;
    const formValue = this.eventForm.value;
    const request = this.isEdit
      ? this.eventService.updateEvent(this.eventId, formValue)
      : this.eventService.createEvent(formValue);
    request.subscribe({
      next: () => {
        this.loading = false;
        this.router.navigate(['/admin/events']);
      },
      error: (err) => {
        this.loading = false;
        this.error = err.error?.message || 'Operation failed';
      }
    });
  }
}