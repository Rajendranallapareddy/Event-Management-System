import { Component, OnInit } from '@angular/core';
import { RegistrationService } from '../../services/registration.service';
import { Registration } from '../../models/registration.model';

@Component({
  selector: 'app-admin-attendance',
  templateUrl: './admin-attendance.component.html'
})
export class AdminAttendanceComponent implements OnInit {
  registrations: Registration[] = [];
  loading = true;
  error = '';

  constructor(private registrationService: RegistrationService) {}

  ngOnInit(): void {
    this.loadRegistrations();
  }

  loadRegistrations(): void {
    this.loading = true;
    this.registrationService.getAllRegistrations(1, 100).subscribe({
      next: (res) => {
        this.registrations = res.items;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.error = 'Failed to load registrations';
        this.loading = false;
      }
    });
  }

  markAttendance(regId: number): void {
    this.registrationService.markAttendance(regId).subscribe({
      next: () => {
        const reg = this.registrations.find(r => r.registrationId === regId);
        if (reg) reg.attendanceStatus = true;
        alert('Attendance marked successfully');
      },
      error: () => alert('Failed to mark attendance')
    });
  }
}