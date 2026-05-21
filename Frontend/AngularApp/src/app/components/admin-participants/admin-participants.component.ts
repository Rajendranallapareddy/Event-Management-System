import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';
import { PagedResult } from '../../models/paged-result.model';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-admin-participants',
  templateUrl: './admin-participants.component.html'
})
export class AdminParticipantsComponent implements OnInit {
  participants: User[] = [];
  loading = true;
  error = '';

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    this.loadParticipants();
  }

  loadParticipants(): void {
    this.userService.getAllUsers(1, 100).subscribe({
      next: (res: PagedResult<User>) => {
        this.participants = res.items.filter((u: User) => u.role === 'Participant');
        this.loading = false;
      },
      error: (err: HttpErrorResponse) => {
        console.error(err);
        this.error = 'Failed to load participants';
        this.loading = false;
      }
    });
  }
}