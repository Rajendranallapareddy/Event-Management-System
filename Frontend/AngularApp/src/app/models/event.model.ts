export interface Event {
  eventId: number;
  title: string;
  description: string;
  location: string;
  startDate: Date;
  endDate: Date;
  capacity: number;
  imageUrl?: string;
  sessionCount: number;
}