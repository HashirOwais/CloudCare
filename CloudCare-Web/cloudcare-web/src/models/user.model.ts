export interface User {
  id: number;
  auth0Id: string;
  email: string;
  name: string;
  daycareName: string;
  daycareAddress: string;
  phoneNumber?: string;
  websiteUrl?: string;
  notes?: string;
  role: string;
  userCreated: string; // ISO string
} 