import { Component, OnInit } from '@angular/core';
import { RegistrationService } from '../../services/registration.service';
import { Registration } from '../../models/registration.model';

@Component({
  selector: 'app-attendance',
  templateUrl: './attendance.component.html',
  styleUrls: ['./attendance.component.css']
})
export class AttendanceComponent implements OnInit {
  registrations: Registration[] = [];
  loading = true;

  constructor(private registrationService: RegistrationService) {}

  ngOnInit(): void {
    this.registrationService.getMyEvents(1, 100).subscribe({
      next: (res) => { this.registrations = res.items; this.loading = false; }
    });
  }

  markAttendance(id: number): void {
    this.registrationService.markAttendance(id).subscribe({
      next: () => {
        const reg = this.registrations.find(r => r.registrationId === id);
        if (reg) reg.attendanceStatus = true;
      }
    });
  }
}