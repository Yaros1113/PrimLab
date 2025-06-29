export interface Client {
  id: number;
  name: string;
  email: string;
  registrationDate: Date;
  phoneNumbers: string[];
  userId?: number;
}