export interface Registration {
  registrationId: number;
  eventId: number;
  eventTitle: string;
  participantEmail?: string;
  registrationDate: Date;
  attendanceStatus: boolean;
}