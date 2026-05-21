export interface Session {
  sessionId: number;
  eventId: number;
  eventTitle: string;
  title: string;
  description: string;
  speakerId?: number;
  speakerName?: string;
  sessionStart: Date;
  sessionEnd: Date;
  sessionUrl?: string;
}