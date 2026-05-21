import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { EventListComponent } from './components/event-list/event-list.component';
import { EventDetailsComponent } from './components/event-details/event-details.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { EventFormComponent } from './components/event-form/event-form.component';
import { AdminEventsComponent } from './components/admin-events/admin-events.component';
import { AdminSpeakersComponent } from './components/admin-speakers/admin-speakers.component';
import { AdminSessionsComponent } from './components/admin-sessions/admin-sessions.component';
import { RegisteredEventsComponent } from './components/registered-events/registered-events.component';
import { AdminAttendanceComponent } from './components/admin-attendance/admin-attendance.component';
import { AdminParticipantsComponent } from './components/admin-participants/admin-participants.component';
import { AuthGuard } from './guards/auth.guard';
import { RoleGuard } from './guards/role.guard';
import { DeveloperComponent } from './components/developer/developer.component';


const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'events', component: EventListComponent },
  { path: 'events/:id', component: EventDetailsComponent },
  { path: 'admin/dashboard', component: DashboardComponent, canActivate: [AuthGuard, RoleGuard], data: { expectedRole: 'Admin' } },
  { path: 'admin/events', component: AdminEventsComponent, canActivate: [AuthGuard, RoleGuard], data: { expectedRole: 'Admin' } },
  { path: 'admin/events/create', component: EventFormComponent, canActivate: [AuthGuard, RoleGuard], data: { expectedRole: 'Admin' } },
  { path: 'admin/events/edit/:id', component: EventFormComponent, canActivate: [AuthGuard, RoleGuard], data: { expectedRole: 'Admin' } },
  { path: 'admin/speakers', component: AdminSpeakersComponent, canActivate: [AuthGuard, RoleGuard], data: { expectedRole: 'Admin' } },
  { path: 'admin/sessions', component: AdminSessionsComponent, canActivate: [AuthGuard, RoleGuard], data: { expectedRole: 'Admin' } },
  { path: 'admin/attendance', component: AdminAttendanceComponent, canActivate: [AuthGuard, RoleGuard], data: { expectedRole: 'Admin' } },
  { path: 'participant/events', component: RegisteredEventsComponent, canActivate: [AuthGuard, RoleGuard], data: { expectedRole: 'Participant' }},
  { path: 'admin/participants', component: AdminParticipantsComponent, canActivate: [AuthGuard, RoleGuard], data: { expectedRole: 'Admin' } },
  { path: 'developer', component: DeveloperComponent },
  { path: '**', redirectTo: '/home' }
  
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }