export interface Task {
  id: number;
  orderId: number;
  title: string;
  description?: string;
  status: boolean;
  createdDate: Date;
  storeAddress: string;
}