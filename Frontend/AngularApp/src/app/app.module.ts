// src/app/app.module.ts
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';              
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { FooterComponent } from './components/footer/footer.component';
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
import { SessionComponent } from './components/sessions/session.component';
import { SpeakerComponent } from './components/speakers/speaker.component';
import { RegisteredEventsComponent } from './components/registered-events/registered-events.component';
import { AttendanceComponent } from './components/attendance/attendance.component';
import { NotFoundComponent } from './components/not-found/not-found.component';
import { TokenInterceptor } from './interceptors/token.interceptor';
import { AdminAttendanceComponent } from './components/admin-attendance/admin-attendance.component';
import { AdminParticipantsComponent } from './components/admin-participants/admin-participants.component';
import { DeveloperComponent } from './components/developer/developer.component';



@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    FooterComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    EventListComponent,
    EventDetailsComponent,
    DashboardComponent,
    EventFormComponent,
    AdminEventsComponent,
    AdminSpeakersComponent,
    AdminSessionsComponent,
    AdminAttendanceComponent,
    AdminParticipantsComponent,
    SessionComponent,          
    SpeakerComponent,          
    RegisteredEventsComponent,
    AttendanceComponent,
    NotFoundComponent,
    DeveloperComponent
  ],
  imports: [
    BrowserModule,
    CommonModule,              
    AppRoutingModule,
    HttpClientModule,
    FormsModule,                
    ReactiveFormsModule
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }